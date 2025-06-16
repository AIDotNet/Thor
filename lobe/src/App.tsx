import './App.css'
import FullscreenLoading from './components/Loading'
import { ThemeProvider } from '@lobehub/ui'
import MainLayout from './_layout'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import { lazy, Suspense, useEffect } from 'react'
import { useTranslation } from 'react-i18next'
import PwaWrapper from './components/PwaWrapper'
import { updatePwaThemeColor, listenForThemeUpdates } from './utils/pwa'

import Nav from './components/@nav/default'
import useThemeStore from './store/theme'
const ProductPage = lazy(() => import('./pages/product/page'))
const LoggerPage = lazy(() => import('./pages/logger/page'))
const RedeemCodePage = lazy(() => import('./pages/redeem-code/page'))
const UserPage = lazy(() => import('./pages/user/page'))
const CurrentPage = lazy(() => import('./pages/current/page'))
const SettingPage = lazy(() => import('./pages/setting/page'))
const LoginPage = lazy(() => import('./pages/login/page'))
const RegisterPage = lazy(() => import('./pages/register/page'))
const ModelPage = lazy(() => import('./pages/model/page'))
const Auth = lazy(() => import('./pages/auth/page'))
const RateLimit = lazy(() => import('./pages/rate-limit/page'))
const WelcomePage = lazy(() => import('./pages/welcome/page'))
const DefaultLayout = lazy(() => import('./_layout/Default/page'))
const ChannelPage = lazy(() => import('./pages/channel/page'))
const TokenPage = lazy(() => import('./pages/token/page'))
const ModelManager = lazy(() => import('./pages/model-manager/page'))
const PanelPage = lazy(() => import('./pages/panel/page'))
const ModelMapPage = lazy(() => import('./pages/model-map/page'))
const UserGroupPage = lazy(() => import('./pages/user-group/page'))
const PlaygroundPage = lazy(() => import('./pages/playground'))
const UsagePage = lazy(() => import('./pages/usage/page'))
const AnnouncementPage = lazy(() => import('./pages/announcement/page'))

function App() {
  const { themeMode, toggleTheme } = useThemeStore();
  const { t } = useTranslation();

  // 在应用启动和主题变化时同步PWA主题色
  useEffect(() => {
    updatePwaThemeColor(themeMode);
  }, [themeMode]);
  
  // 监听来自Service Worker的主题更新消息
  useEffect(() => {
    // 仅在支持Service Worker的环境中启用
    if ('serviceWorker' in navigator) {
      const cleanup = listenForThemeUpdates((updatedThemeMode) => {
        // 如果收到主题更新消息，更新应用主题
        if (updatedThemeMode !== themeMode) {
          toggleTheme(updatedThemeMode);
        }
      });
      
      return cleanup;
    }
  }, [themeMode, toggleTheme]);

  const router = createBrowserRouter([{
    element: <MainLayout nav={<Nav />} />,
    children: [
      {
        path: 'panel', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.panel')} />}>
          <PanelPage />
        </Suspense>
      },
      {
        path: 'channel', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.channel')} />}>
          <ChannelPage />
        </Suspense>
      },
      {
        path: 'token', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.token')} />}>
          <TokenPage />
        </Suspense>
      },
      {
        path: 'model-manager', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.modelManager')} />}>
          <ModelManager />
        </Suspense>
      },
      {
        path: 'product', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.product')} />}>
          <ProductPage />
        </Suspense>
      },
      {
        path: 'logger', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.logger')} />}>
          <LoggerPage />
        </Suspense>
      },
      {
        path: 'redeem-code', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.redeemCode')} />}>
          <RedeemCodePage />
        </Suspense>
      },
      {
        path: 'user', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.user')} />}>
          <UserPage />
        </Suspense>
      },
      {
        path: 'current', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.current')} />}>
          <CurrentPage />
        </Suspense>
      },
      {
        path: 'setting', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.setting')} />}>
          <SettingPage />
        </Suspense>
      },
      {
        path: 'rate-limit', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.rateLimit')} />}>
          <RateLimit />
        </Suspense>
      },
      {
        path: 'model-map', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.modelMap')} />}>
          <ModelMapPage />
        </Suspense>
      },
      {
        path: 'user-group', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.userGroup')} />}>
          <UserGroupPage />
        </Suspense>
      },
      {
        path: 'playground', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.playground')} />}>
          <PlaygroundPage />
        </Suspense>
      },
      {
        path: 'usage', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.usage')} />}>
          <UsagePage />
        </Suspense>
      },
      {
        path: 'announcement', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.announcement')} />}>
          <AnnouncementPage />
        </Suspense>
      }
    ]
  }, {
    path: "/login",
    element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.login')} />}>
      <LoginPage />
    </Suspense>
  }, {
    path: "/register",
    element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.register')} />}>
      <RegisterPage />
    </Suspense>
  }, {
    path: "/auth",
    element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.auth')} />}>
      <Auth />
    </Suspense>
  }, {
    path: "/auth/gitee",
    element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.auth')} />}>
      <Auth />
    </Suspense>
  }, {
    path: "/auth/casdoor",
    element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.auth')} />}>
      <Auth />
    </Suspense>
  },
  {
    element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.defaultLayout')} />}>
      <DefaultLayout />
    </Suspense>,
    children: [
      {
        path: '', element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.welcome')} />}>
          <WelcomePage />
        </Suspense>
      }, {
        path: "/model",
        element: <Suspense fallback={<FullscreenLoading title={t('pageTitle.loading.model')} />}>
          <ModelPage />
        </Suspense>
      },
    ]
  },
  ]);

  return (
    <ThemeProvider themeMode={themeMode}
      onThemeModeChange={(mode) => {
        toggleTheme(mode)
      }}
      style={{
        height: '100%'
      }}>
      <PwaWrapper>
        <RouterProvider router={router} />
      </PwaWrapper>
    </ThemeProvider>
  )
}

export default App