// Конфигурация для Vite (отдельно от основного app config)
export const VITE_CONFIG = {
    server: {
      port: 5173,
      host: 'localhost', 
      strictPort: true
    },
    proxy: {
      target: 'https://localhost:5092'
    }
  };
  
  export default VITE_CONFIG;