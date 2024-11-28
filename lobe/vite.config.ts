import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  build:{
    target: ['edge90', 'chrome90', 'firefox90', 'safari15'],
  },
  server:{
    proxy: {
      '/api': {
        target: 'http://localhost:5045',
        changeOrigin: true,
      }
    }
  }
})
