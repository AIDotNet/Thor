import './App.css'
import FullscreenLoading from './components/Loading'
import { ThemeProvider } from '@lobehub/ui'
import MainLayout from './_layout'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import { lazy, Suspense } from 'react'

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
const DocPage = lazy(() => import('./pages/doc/page'))
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

const router = createBrowserRouter([{
  element: <MainLayout nav={<Nav />} />,
  children: [
    {
      path: 'panel', element: <Suspense fallback={<FullscreenLoading title='加载面板中' />}>
        <PanelPage />
      </Suspense>
    },
    {
      path: 'channel', element: <Suspense fallback={<FullscreenLoading title='加载渠道中' />}>
        <ChannelPage />
      </Suspense>
    },
    {
      path: 'token', element: <Suspense fallback={<FullscreenLoading title='加载Token管理中' />}>
        <TokenPage />
      </Suspense>
    },
    {
      path: 'model-manager', element: <Suspense fallback={<FullscreenLoading title='加载模型管理中' />}>
        <ModelManager />
      </Suspense>
    },
    {
      path: 'product', element: <Suspense fallback={<FullscreenLoading title='加载产品页面中' />}>
        <ProductPage />
      </Suspense>
    },
    {
      path: 'logger', element: <Suspense fallback={<FullscreenLoading title='加载日志页面中' />}>
        <LoggerPage />
      </Suspense>
    },
    {
      path: 'redeem-code', element: <Suspense fallback={<FullscreenLoading title='加载兑换码页面中' />}>
        <RedeemCodePage />
      </Suspense>
    },
    {
      path: 'user', element: <Suspense fallback={<FullscreenLoading title='加载用户页面中' />}>
        <UserPage />
      </Suspense>
    },
    {
      path: 'current', element: <Suspense fallback={<FullscreenLoading title='加载当前页面中' />}>
        <CurrentPage />
      </Suspense>
    },
    {
      path: 'setting', element: <Suspense fallback={<FullscreenLoading title='加载设置页面中' />}>
        <SettingPage />
      </Suspense>
    },
    {
      path: 'rate-limit', element: <Suspense fallback={<FullscreenLoading title='加载限速页面中' />}>
        <RateLimit />
      </Suspense>
    },
    {
      path: 'model-map', element: <Suspense fallback={<FullscreenLoading title='加载模型映射页面中' />}>
        <ModelMapPage />
      </Suspense>
    },
    {
      path: 'user-group', element: <Suspense fallback={<FullscreenLoading title='加载用户分组页面中' />}>
        <UserGroupPage />
      </Suspense>
    }
  ]
}, {
  path: "/login",
  element: <Suspense fallback={<FullscreenLoading title='加载登录页面中' />}>
    <LoginPage />
  </Suspense>
}, {
  path: "/register",
  element: <Suspense fallback={<FullscreenLoading title='加载注册页面中' />}>
    <RegisterPage />
  </Suspense>
}, {
  path: "/auth",
  element: <Suspense fallback={<FullscreenLoading title='加载认证页面中' />}>
    <Auth />
  </Suspense>
}, {
  path: "/auth/gitee",
  element: <Suspense fallback={<FullscreenLoading title='加载认证页面中' />}>
    <Auth />
  </Suspense>
}, {
  path: "/auth/casdoor",
  element: <Suspense fallback={<FullscreenLoading title='加载认证页面中' />}>
    <Auth />
  </Suspense>
},
{
  element: <Suspense fallback={<FullscreenLoading title='加载默认布局中' />}>
    <DefaultLayout />
  </Suspense>,
  children: [
    {
      path: '', element: <Suspense fallback={<FullscreenLoading title='加载欢迎页面中' />}>
        <WelcomePage />
      </Suspense>
    },
    {
      path: "/doc",
      element: <Suspense fallback={<FullscreenLoading title='加载文档页面中' />}>
        <DocPage />
      </Suspense>
    }, {
      path: "/doc/*",
      element: <Suspense fallback={<FullscreenLoading title='加载文档页面中' />}>
        <DocPage />
      </Suspense>
    }, {
      path: "/model",
      element: <Suspense fallback={<FullscreenLoading title='加载模型页面中' />}>
        <ModelPage />
      </Suspense>
    },
  ]
},

])

function App() {
  const { themeMode, toggleTheme } = useThemeStore();
  return (
    <ThemeProvider themeMode={themeMode}
      onThemeModeChange={(mode) => {
        toggleTheme(mode)
      }}
      style={{
        height: '100%'
      }}>
      <RouterProvider router={router} />
    </ThemeProvider>
  )
}

export default App