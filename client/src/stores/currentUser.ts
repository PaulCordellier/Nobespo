import { defineStore } from 'pinia'
import { useLocalStorage } from '@vueuse/core'

import { type Account } from "@/models/account"

export const useCurrentUserStore = defineStore('auth', {
    state: () => ({
        username: useLocalStorage('username', ''),
        id: useLocalStorage('userId', 0),
        token: useLocalStorage('userIsConnected', ''),
        isConnected: useLocalStorage('isConnected', false),
    }),
    actions: {
        saveUserData(account : Account, token : string) {
            this.username = account.username
            this.id = account.id
            this.token = token
            this.isConnected = true
        },
        disconnectUser() {
            this.username = ''
            this.id = 0
            this.token = ''
            this.isConnected = false
        }
    }
})