// Service Worker Version
const CACHE_VERSION = 'v1.1';
const CACHE_NAME = `thor-ai-cache-${CACHE_VERSION}`;

// 主题色定义
const THEME_COLORS = {
  light: '#1890ff',
  dark: '#177ddc'
};

// Assets to cache
const STATIC_ASSETS = [
  '/',
  '/index.html',
  '/offline.html',
  '/manifest.json',
  '/logo.png',
  '/service-worker.js'
];

// Install Event - Cache Static Assets
self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(CACHE_NAME).then((cache) => {
      return cache.addAll(STATIC_ASSETS);
    }).then(() => {
      return self.skipWaiting();
    })
  );
});

// Activate Event - Clean Old Caches
self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys().then((cacheNames) => {
      return Promise.all(
        cacheNames.map((cacheName) => {
          if (cacheName !== CACHE_NAME) {
            return caches.delete(cacheName);
          }
        })
      );
    }).then(() => {
      return self.clients.claim();
    })
  );
});

// Fetch Event - Network First with Cache Fallback Strategy
self.addEventListener('fetch', (event) => {
  // 处理主题偏好同步消息
  if (event.request.url.includes('/theme-preference')) {
    return;
  }

  // Skip cross-origin requests
  if (!event.request.url.startsWith(self.location.origin)) {
    return;
  }

  // Skip non-GET requests
  if (event.request.method !== 'GET') {
    return;
  }

  event.respondWith(
    fetch(event.request)
      .then((response) => {
        // If the response is valid, clone it and store in cache
        if (response && response.status === 200) {
          const responseClone = response.clone();
          caches.open(CACHE_NAME).then((cache) => {
            cache.put(event.request, responseClone);
          });
        }
        return response;
      })
      .catch(() => {
        // If network request fails, try to retrieve from cache
        return caches.match(event.request).then((cachedResponse) => {
          if (cachedResponse) {
            return cachedResponse;
          }
          
          // If the request is for an HTML page, return the offline page
          if (event.request.headers.get('accept')?.includes('text/html')) {
            return caches.match('/offline.html');
          }
          
          // If we can't recover, just return an error response
          return new Response('网络连接失败', {
            status: 503,
            statusText: 'Service Unavailable',
            headers: new Headers({
              'Content-Type': 'text/plain'
            })
          });
        });
      })
  );
});

// Message Event - 处理主题颜色更新等消息
self.addEventListener('message', (event) => {
  if (event.data && event.data.type === 'THEME_CHANGED') {
    const { themeMode } = event.data;
    
    // 存储主题模式到IndexedDB或其他存储中
    storeThemePreference(themeMode);
    
    // 通知所有客户端主题已改变
    self.clients.matchAll().then(clients => {
      clients.forEach(client => {
        if (client.id !== event.source.id) {
          client.postMessage({
            type: 'THEME_UPDATED',
            themeMode
          });
        }
      });
    });
  }
});

// 存储主题偏好
async function storeThemePreference(themeMode) {
  // 简单实现，实际应用中应使用IndexedDB
  try {
    const cache = await caches.open(CACHE_NAME);
    await cache.put(
      new Request('/theme-preference'),
      new Response(JSON.stringify({ themeMode }))
    );
  } catch (error) {
    console.error('Failed to store theme preference:', error);
  }
}

// 读取存储的主题偏好
async function getStoredThemePreference() {
  try {
    const cache = await caches.open(CACHE_NAME);
    const response = await cache.match('/theme-preference');
    if (response) {
      const data = await response.json();
      return data.themeMode;
    }
  } catch (error) {
    console.error('Failed to retrieve theme preference:', error);
  }
  return null;
}

// Background Sync for Offline Data
self.addEventListener('sync', (event) => {
  if (event.tag === 'sync-data') {
    event.waitUntil(syncData());
  }
});

// Function to sync data when back online
async function syncData() {
  try {
    // Get all pending requests from IndexedDB or other storage
    const pendingRequests = await getPendingRequests();
    
    // Process each pending request
    for (const request of pendingRequests) {
      await fetch(request.url, {
        method: request.method,
        headers: request.headers,
        body: request.body
      });
      
      // Remove processed request from storage
      await removePendingRequest(request.id);
    }
    
    // Notify the user that sync is complete
    self.registration.showNotification('Thor AI 平台', {
      body: '数据已成功同步',
      icon: '/logo.png'
    });
    
    return true;
  } catch (error) {
    console.error('Background sync failed:', error);
    return false;
  }
}

// Placeholder functions for data storage
// These would need to be implemented with IndexedDB in a real application
async function getPendingRequests() {
  // In a real app, get from IndexedDB
  return [];
}

async function removePendingRequest(id) {
  // In a real app, remove from IndexedDB
  return true;
} 