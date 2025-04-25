/**
 * PWA-related TypeScript definitions
 */

// BeforeInstallPromptEvent interface
interface BeforeInstallPromptEvent extends Event {
  /**
   * Returns an array of DOMString items containing the platforms on which the event was dispatched.
   * This is provided for user agents that want to present a choice of versions to the user.
   */
  readonly platforms: Array<string>;

  /**
   * Returns a Promise that resolves to a DOMString containing either "accepted" or "dismissed".
   */
  readonly userChoice: Promise<{
    outcome: 'accepted' | 'dismissed';
    platform: string;
  }>;

  /**
   * Allows a developer to show the install prompt at a time of their own choosing.
   * This method returns a Promise.
   */
  prompt(): Promise<void>;
}

// Extend the Window interface
interface Window {
  /**
   * The beforeinstallprompt event is fired when a user is about to be prompted to "install" a web application.
   */
  onbeforeinstallprompt: ((this: Window, ev: BeforeInstallPromptEvent) => any) | null;
} 