import { defineStore } from 'pinia'
import { useLocalStorage } from '@vueuse/core'

export const useCurrentUserStore = defineStore('auth', {
    state: () => ({
        username: useLocalStorage('username', ''),
        token: useLocalStorage('userTocken', ''),
        isConnected: useLocalStorage('isConnected', false),
    }),
    actions: {
        saveUserData(username : string, token : string) {
            this.username = username
            this.token = token
            this.isConnected = true
        },
        disconnectUser() {
            this.username = ''
            this.token = ''
            this.isConnected = false
        }
    }
})