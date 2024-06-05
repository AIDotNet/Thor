
import './App.css'
import { ThemeProvider } from '@lobehub/ui'
import MainLayout from './_layout'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import PanelPage from './pages/panel/page'
import Nav from './components/@nav/default'
import ChannelPage from './pages/channel/page'
import TokenPage from './pages/token/page'
import ProductPage from './pages/product/page'
import LoggerPage from './pages/logger/page'
import RedeemCodePage from './pages/redeem-code/page'
import UserPage from './pages/user/page'
import CurrentPage from './pages/current/page'
import SettingPage from './pages/setting/page'
import LoginPage from './pages/login/page'
import RegisterPage from './pages/register/page'
import DocPage from './pages/doc/page'
import ModelPage from './pages/model/page'
import DocMainLayout from './_layout/Home/@nav/default'
import Auth from './pages/auth/page'
import RateLimit from './pages/rate-limit/page'


const router = createBrowserRouter([{
  element: <MainLayout nav={<Nav />} />,
  children: [
    { path: '', element: <PanelPage /> },
    { path: 'panel', element: <PanelPage /> },
    { path: 'channel', element: <ChannelPage /> },
    { path: 'token', element: <TokenPage /> },
    {
      path: 'product',
      element: <ProductPage />
    },
    {
      path: 'logger',
      element: <LoggerPage />
    },
    {
      path: 'redeem-code',
      element: <RedeemCodePage />
    },
    {
      path: 'user',
      element: <UserPage />
    },
    {
      path: 'current',
      element: <CurrentPage />
    },
    {
      path: 'setting',
      element: <SettingPage />
    },
    {
      path: 'rate-limit',
      element: <RateLimit />
    }
  ]
}, {
  path: "/login",
  element: <LoginPage />
}, {
  path: "/register",
  element: <RegisterPage />
}, {
  path: "/doc",
  element: <DocPage nav={<DocMainLayout />} />
}, {
  path: "/model",
  element: <ModelPage nav={<DocMainLayout />} />
}, {
  path: "/auth",
  element: <Auth />
}
])

function App() {
  return (
    <ThemeProvider themeMode='auto' style={{
      height: '100%'
    }}>
      <RouterProvider router={router} />
    </ThemeProvider>
  )
}

export default App
