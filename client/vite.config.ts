import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// docs : https://vite.dev/config/
export default defineConfig({
    plugins: [
        vue(),
        vueDevTools()
    ],
    resolve: {
        alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url))
        },
    },
    server: {
        proxy: {
            '/api': {
                target: 'http://localhost:5259',
                rewrite: (path) => path.replace(/^\/api/, ''),
                secure: false
            },
        }
    },
    css: {      // Docs : https://vite.dev/config/shared-options.html#css-preprocessoroptions
        preprocessorOptions: {
            scss: {
                api: 'modern-compiler'
            },
        },
    },
    base: "/"
})
