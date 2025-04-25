
/**
 * PWA主题色值
 */
const PWA_THEME_COLORS = {
  light: '#1890ff', // Ant Design的蓝色主题
  dark: '#177ddc',  // Ant Design暗色模式下的蓝色
};

/**
 * 更新PWA的主题色以匹配应用当前主题
 * @param themeMode 当前应用的主题模式
 */
export function updatePwaThemeColor(themeMode: 'light' | 'dark' | 'auto'): void {
  // 获取系统主题偏好
  const prefersDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
  
  // 确定最终使用的主题模式
  let effectiveTheme: 'light' | 'dark';
  
  if (themeMode === 'auto') {
    effectiveTheme = prefersDarkMode ? 'dark' : 'light';
  } else {
    effectiveTheme = themeMode;
  }
  
  // 更新主题色meta标签
  const themeColorMeta = document.querySelector('meta[name="theme-color"]:not([media])');
  if (themeColorMeta) {
    themeColorMeta.setAttribute('content', PWA_THEME_COLORS[effectiveTheme]);
  }
  
  // 如果在standalone模式下，尝试更新状态栏颜色（仅部分平台支持）
  if (isPwaInstalled() && 'navigator' in window && 'theme' in navigator) {
    try {
      // @ts-ignore - 这是一个实验性API，TypeScript没有定义
      navigator.theme.color = PWA_THEME_COLORS[effectiveTheme];
    } catch (error) {
      // 忽略不支持的平台错误
    }
  }
  
  // 通知Service Worker主题已更改
  notifyServiceWorkerThemeChanged(themeMode);
}

/**
 * 将主题变更通知到Service Worker
 * @param themeMode 当前的主题模式
 */
async function notifyServiceWorkerThemeChanged(themeMode: 'light' | 'dark' | 'auto'): Promise<void> {
  if (!('serviceWorker' in navigator)) {
    return;
  }
  
  try {
    const registration = await navigator.serviceWorker.ready;
    if (registration.active) {
      registration.active.postMessage({
        type: 'THEME_CHANGED',
        themeMode
      });
    }
  } catch (error) {
    console.error('Failed to notify service worker about theme change:', error);
  }
}

/**
 * 监听来自Service Worker的主题更新消息
 * @param callback 收到更新时的回调函数
 */
export function listenForThemeUpdates(
  callback: (themeMode: 'light' | 'dark' | 'auto') => void
): () => void {
  const messageHandler = (event: MessageEvent) => {
    if (event.data && event.data.type === 'THEME_UPDATED') {
      callback(event.data.themeMode);
    }
  };
  
  navigator.serviceWorker.addEventListener('message', messageHandler);
  
  // 返回清理函数
  return () => {
    navigator.serviceWorker.removeEventListener('message', messageHandler);
  };
}

/**
 * Check if the app is installed as a PWA
 */
export function isPwaInstalled(): boolean {
  // Check if the app is in standalone mode (PWA installed)
  return window.matchMedia('(display-mode: standalone)').matches || 
         (window.navigator as any).standalone === true;
}

/**
 * Checks if the browser supports installing PWAs
 */
export function isPwaSupported(): boolean {
  return 'serviceWorker' in navigator && 'PushManager' in window;
}

/**
 * Attempts to process any queued API requests
 * This is a simpler alternative to using the background sync API
 */
export async function processQueuedRequests(): Promise<boolean> {
  if (!navigator.onLine) {
    return false;
  }
  
  try {
    const pendingRequestsStr = localStorage.getItem('pendingApiRequests');
    if (!pendingRequestsStr) {
      return true;
    }
    
    const pendingRequests = JSON.parse(pendingRequestsStr);
    if (!pendingRequests.length) {
      return true;
    }
    
    const successfulIds: string[] = [];
    
    // Process each pending request
    for (const request of pendingRequests) {
      try {
        await fetch(request.url, {
          method: request.method,
          headers: request.headers,
          body: request.body ? JSON.parse(request.body) : null
        });
        
        successfulIds.push(request.id);
      } catch (error) {
        console.error('Failed to process queued request:', error);
      }
    }
    
    // Remove processed requests
    if (successfulIds.length) {
      const remainingRequests = pendingRequests.filter(
        (req: any) => !successfulIds.includes(req.id)
      );
      localStorage.setItem('pendingApiRequests', JSON.stringify(remainingRequests));
    }
    
    return true;
  } catch (error) {
    console.error('Failed to process queued requests:', error);
    return false;
  }
}

/**
 * Requests permission for push notifications
 */
export async function requestNotificationPermission(): Promise<NotificationPermission> {
  if (!('Notification' in window)) {
    console.warn('This browser does not support notifications');
    return 'denied';
  }
  
  if (Notification.permission === 'granted') {
    return 'granted';
  }
  
  if (Notification.permission !== 'denied') {
    return await Notification.requestPermission();
  }
  
  return 'denied';
}

/**
 * Checks the online status of the application
 */
export function isOnline(): boolean {
  return navigator.onLine;
}

/**
 * Adds event listeners for online/offline events
 * @param onlineCallback Function to call when app goes online
 * @param offlineCallback Function to call when app goes offline
 */
export function addConnectivityListeners(
  onlineCallback: () => void, 
  offlineCallback: () => void
): () => void {
  window.addEventListener('online', onlineCallback);
  window.addEventListener('offline', offlineCallback);
  
  // Return a cleanup function
  return () => {
    window.removeEventListener('online', onlineCallback);
    window.removeEventListener('offline', offlineCallback);
  };
}

/**
 * Displays an installation prompt for PWA
 * Note: This should be called in response to a user action
 * @param beforeInstallPromptEvent The beforeinstallprompt event saved earlier
 */
export async function showInstallPrompt(beforeInstallPromptEvent: BeforeInstallPromptEvent): Promise<boolean> {
  if (!beforeInstallPromptEvent) {
    return false;
  }
  
  // Show the install prompt
  beforeInstallPromptEvent.prompt();
  
  // Wait for the user to respond to the prompt
  const choiceResult = await beforeInstallPromptEvent.userChoice;
  
  // Return true if the user accepted the installation
  return choiceResult.outcome === 'accepted';
}

/**
 * Store data for offline use (using LocalStorage for simplicity)
 * In a real app, you would use IndexedDB for more complex data
 * @param key Storage key
 * @param data Data to store
 */
export function storeOfflineData(key: string, data: any): void {
  try {
    localStorage.setItem(key, JSON.stringify(data));
  } catch (error) {
    console.error('Failed to store offline data:', error);
  }
}

/**
 * Retrieve data stored for offline use
 * @param key Storage key
 */
export function retrieveOfflineData<T>(key: string): T | null {
  try {
    const data = localStorage.getItem(key);
    return data ? JSON.parse(data) : null;
  } catch (error) {
    console.error('Failed to retrieve offline data:', error);
    return null;
  }
}

/**
 * Queues an API request for execution when back online
 * @param url The request URL
 * @param method The HTTP method
 * @param body The request body
 * @param headers The request headers
 */
export function queueApiRequest(
  url: string,
  method: string = 'GET',
  body: any = null,
  headers: HeadersInit = {}
): void {
  // Simple implementation using localStorage
  // In a real app, use IndexedDB with a more robust solution
  try {
    const pendingRequests = JSON.parse(localStorage.getItem('pendingApiRequests') || '[]');
    
    pendingRequests.push({
      id: Date.now().toString(),
      url,
      method,
      body: body ? JSON.stringify(body) : null,
      headers,
      timestamp: Date.now()
    });
    
    localStorage.setItem('pendingApiRequests', JSON.stringify(pendingRequests));
    
    // Try to process requests immediately if online
    if (navigator.onLine) {
      processQueuedRequests();
    }
  } catch (error) {
    console.error('Failed to queue API request:', error);
  }
} 