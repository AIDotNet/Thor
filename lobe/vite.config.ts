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
    host: true,
    port: 5170,
    proxy: {
      '/api': {
        target: 'http://localhost:5045',
        changeOrigin: true,
      },
      '/v1': {
        target: 'http://localhost:5045',
        changeOrigin: true,
      }
    }
  }
})
