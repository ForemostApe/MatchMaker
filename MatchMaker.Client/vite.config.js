import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';
import fs from 'fs';
import path from 'path';

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), 'VITE_');

  const isHttps = fs.existsSync(path.resolve(__dirname, 'localhost-key.pem'));

  return {
    plugins: [react(), tailwindcss()],

    define: {
      'import.meta.env.VITE_API_BASE': JSON.stringify(env.VITE_API_BASE),
    },

    server: {
      https: isHttps
        ? {
            key: fs.readFileSync(path.resolve(__dirname, 'localhost-key.pem')),
            cert: fs.readFileSync(path.resolve(__dirname, 'localhost.pem')),
          }
        : false,
      port: 5173,
      host: 'localhost',

      proxy: {
        '/api': {
          target: env.VITE_API_PROXY || 'https://localhost:5001',
          changeOrigin: true,
          secure: false,
          rewrite: (path) => path.replace(/^\/api/, ''),
        },
      },
    },

    build: {
      // outDir: '../MatchMaker.Api/wwwroot',
      outDir: path.resolve('C:/inetpub/wwwroot'),
      emptyOutDir: true,
    },
  };
});
