
import './App.css'
import { ThemeProvider } from '@lobehub/ui'
import MainLayout from './_layout'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import PanelPage from './pages/panel/page'
import Nav from './components/@nav/default'


const router = createBrowserRouter([{
  element: <MainLayout nav={<Nav />}/>,
  children: [
    { path: 'panel', element: <PanelPage /> },
    { path: '', element: <PanelPage /> },
  ]
}
])

function App() {
  return (
    <ThemeProvider style={{
      height:'100%'
    }}>
      <RouterProvider router={router} />
    </ThemeProvider>
  )
}

export default App
