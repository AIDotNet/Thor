import './App.css'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import MainLayout from './layouts/MainLayout'

import './App.css'

import Panel from './pages/panel'
import Channel from './pages/channel'
import Current from './pages/current'
import Logger from './pages/logger'
import Setting from './pages/setting'
import Token from './pages/token'
import User from './pages/user'
import Login from './pages/login'
import Home from './pages/home'
import Register from './pages/register'
import RedeemCode from './pages/redeemCode'
import Chat from './pages/chat'
import Product from './pages/product'
import Vidol from './pages/vidol'

const router = createBrowserRouter([{
  element: <MainLayout />,
  children: [
    { path: 'panel', element: <Panel /> },
    { path: 'channel', element: <Channel /> },
    { path: 'current', element: <Current /> },
    { path: 'logger', element: <Logger /> },
    { path: 'setting', element: <Setting /> },
    { path: 'product', element: <Product /> },
    { path: 'token', element: <Token /> },
    { path: 'user', element: <User /> },
    { path: 'redeemCode', element: <RedeemCode /> }
  ]
}, {
  path: '/login',
  element: <Login />
}, {
  path: '/',
  element: <Home />
},
{
  path: '/register', element: <Register />
},
{ path: '/chat', element: <Chat /> },
{ path: '/vidol', element: <Vidol /> },
])


export default function AppPage() {
  return (
    <RouterProvider router={router} />
  )
}
