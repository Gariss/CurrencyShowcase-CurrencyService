import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import fs from 'fs'

export default defineConfig({
  plugins: [react()],
  server: {
    host: '0.0.0.0',
    port: 5173,
    https: {
      key: fs.readFileSync('/etc/ssl/frontend/frontend.key'),
      cert: fs.readFileSync('/etc/ssl/frontend/frontend.crt')
    },
    proxy: {
      '/api': {
        target: 'https://localhost:5092',
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace(/^\/api/, '') // Убираем /api из пути
      }
    }
  }
})