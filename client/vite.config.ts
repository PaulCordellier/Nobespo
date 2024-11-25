import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig({
    plugins: [
        vue({
            template: {
                compilerOptions: {
                    // treat all tags with  md- as custom elements
                    isCustomElement: (tag) => tag.startsWith("md-"),
                    // This is used so the material design library can be used
                },
            },
        }),
        vueDevTools(),  
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
            // configure: (proxy, _options) => {
            //     proxy.on('error', (err, _req, _res) => {
            //         console.log('proxy error', err);
            //     });
            //     proxy.on('proxyReq', (proxyReq, req, _res) => {
            //         console.log('Sending Request to the Target:', req.method, req.url);
            //     });
            //     proxy.on('proxyRes', (proxyRes, req, _res) => {
            //         console.log('Received Response from the Target:', proxyRes.statusCode, req.url);
            //     });
            // },

        },
        // port: 5173,
        // https: {
        //     key:     // You need to fill thoose values to setup https
        //     cert: 
        // }
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
