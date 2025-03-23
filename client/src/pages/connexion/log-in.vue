<script setup lang="ts">
import FieldVerifier from "@/components/FieldVerifier.vue"
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useCurrentUserStore } from "@/stores/currentUser"

const router = useRouter()
const currentUserStore = useCurrentUserStore()

const username = ref<string>("")
const password = ref<string>("")

let userAccountNotFound = ref<boolean>(false)
let unexpectedError = ref<boolean>(false)
let showErrorIcon = ref<boolean>(false)

async function submitFrom() {

    const user = {
        "username": username.value,
        "password": password.value
    }

    const response = await fetch("/api/user/login", {
        method: "POST",
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(user)
    })

    if (response.ok) {
        const currentUserData = await response.json()
        currentUserStore.saveUserData(currentUserData.username, currentUserData.token)

        router.push('/')
    } else if (response.status == 404) {    // If the status code is 404, a username with the password wasn't found
        userAccountNotFound.value = true
        showErrorIcon.value = true
    } else {
        unexpectedError.value = true
        showErrorIcon.value = true
    }
}
</script>

<template>
    <div id="connexion-block">
        <h1>Wilkommen!</h1>

        <label for="username" class="text-field-label">Benutzername:</label>
        <input type="text" v-model="username" id="username" class="text-field">

        <label for="password" class="text-field-label">Passwort:</label>
        <input type="password" v-model="password" id="password" class="text-field">

        <FieldVerifier 
            :isValid="!userAccountNotFound"
            :withErrorIcon="showErrorIcon"
            :hideWhenNoError="true"
            description="Kein Benutzer mit diesem Passwort gefunden"
        />

        <FieldVerifier 
            :isValid="!unexpectedError"
            :withErrorIcon="showErrorIcon"
            :hideWhenNoError="true"
            description="Ein unerwarteter Fehler ist aufgetreten."
        />

        <div class="button-container">
            <button class="filled-button" @click="submitFrom">Einloggen</button>
        </div>
    </div>
</template>
