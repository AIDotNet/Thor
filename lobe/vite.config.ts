import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import viteCompression from 'vite-plugin-compression'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    react(),
    viteCompression({
      verbose: true,
      disable: false,
      threshold: 10240,
      algorithm: 'brotliCompress',
      ext: '.br',
    }),
  ],
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
