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


const router = createBrowserRouter([{
  element: <MainLayout />,
  children: [
    { path: 'panel', element: <Panel /> },
    { path: 'channel', element: <Channel /> },
    { path: 'current', element: <Current /> },
    { path: 'logger', element: <Logger /> },
    { path: 'setting', element: <Setting /> },
    { path: 'token', element: <Token /> },
    { path: 'user', element: <User /> },
  ]
}, {
  path: '/login',
  element: <Login />
}, {
  path: '/',
  element: <Home />
}])


export default function AppPage() {
  return (
    <RouterProvider router={router} />
  )
}
