<script setup lang="ts">
import FieldVerifier from "@/components/FieldVerifier.vue"
import { type FieldVerifierInfo } from "@/components/FieldVerifier.vue"
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useCurrentUserStore } from "@/stores/currentUser"

const router = useRouter()
const currentUserStore = useCurrentUserStore()

const username = ref<string>("")
const password = ref<string>("")
const passwordAgain = ref<string>("")

const showErrorIcon = ref<boolean>(false)

const usernameVerifierInfos :  FieldVerifierInfo[] = [
    {
        isValid: () => username.value.length >= 3,
        description: "Der Benutzername muss aus mindestens drei Zeichen bestehen."
    },
    {
        isValid: () => /^[a-z0-9_-]*$/.test(username.value),
        description: "Der Benutzername darf nur Zahlen, Buchstaben ohne Akzent und Unterstriche enthalten."
    },
    {
        isValid: () => username.value.length <= 25,
        description: "Der Benutzername darf nicht mehr als 25 Zeichen lang sein."
    }
]

const passwordVerifierInfos : FieldVerifierInfo[] = [
    {
        isValid: () => password.value.length >= 8,
        description: "Das Passwort muss mindestens 8 Zeichen lang sein."
    },
    {
        isValid: () => password.value.length <= 16,
        description: "Das Passwort muss nicht länger als 16 Zeichen sein."
    },
    {
        isValid: () => /\d/.test(password.value),
        description: "Das Passwort muss mindestens eine Ziffer haben."
    },
    {
        isValid: () => /[!@#$%^&*(),.?":{}|<>]/.test(password.value),
        description: "Das Passwort muss mindestens ein Sonderzeichen enthalten."
    },
    {
        isValid: () => /[a-zà-ÿ]/.test(password.value),
        description: "Das Passwort muss mindestens einen Kleinbuchstaben enthalten."
    },
    {
        isValid: () => /[A-ZÀ-Ý]/.test(password.value),
        description: "Das Passwort muss mindestens einen Großbuchstaben enthalten."
    }
]

const verifyTwoPasswords : FieldVerifierInfo = {
    isValid: () => password.value == passwordAgain.value,
    description: "Die beiden Passwörter sind nicht die gleichen.",
}

function allFieldsAreValid() : boolean {
    return usernameVerifierInfos.every(x => x.isValid()) 
            && passwordVerifierInfos.every(x => x.isValid())
            && verifyTwoPasswords.isValid()
}

let usernameAlreadyUsed = ref<boolean>(false)
let unexpectedError = ref<boolean>(false)

async function submitFrom() {

    usernameAlreadyUsed.value = false
    unexpectedError.value = false
    showErrorIcon.value = false

    username.value = username.value.trim()

    if (!allFieldsAreValid()) {
        showErrorIcon.value = true
        return
    }

    const user = {
        "username": username.value,
        "password": password.value
    }

    const response = await fetch("/api/user/signup", {
        method: "POST",
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(user)
    })

    if (response.ok) {
        const currentUserData = await response.json()
        currentUserStore.saveUserData(currentUserData.username, currentUserData.token)

        router.push('/')
    } else if (response.status == 409) {    // If the status code is 409, there is a conflict error and the username is already used
        usernameAlreadyUsed.value = true
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
        <FieldVerifier 
            :isValid="!usernameAlreadyUsed"
            :withErrorIcon="showErrorIcon"
            :hideWhenNoError="true"
            description="Der Benutzername wird bereits verwendet."
        />
        <div v-for="usernameVerifierInfo in usernameVerifierInfos" v-bind:key="usernameVerifierInfo.description">
            <FieldVerifier 
                :isValid="usernameVerifierInfo.isValid()"
                :withErrorIcon="showErrorIcon"
                :hideWhenNoError="true"
                :description="usernameVerifierInfo.description"
            />
        </div>

        <label for="password" class="text-field-label">Passwort:</label>
        <input type="password" v-model="password" id="password" class="text-field">
        <div v-for="passwordVerifierInfo in passwordVerifierInfos" v-bind:key="passwordVerifierInfo.description">
            <FieldVerifier 
                :isValid="passwordVerifierInfo.isValid()"
                :withErrorIcon="showErrorIcon"
                :hideWhenNoError="false"
                :description="passwordVerifierInfo.description"
            />
        </div>

        <label for="password-again" class="text-field-label">Passwort überprüfen:</label>
        <input type="password" v-model="passwordAgain" id="password-again" class="text-field">

        <FieldVerifier 
            :isValid="verifyTwoPasswords.isValid()"
            :withErrorIcon="showErrorIcon"
            :hideWhenNoError="true"
            :description="verifyTwoPasswords.description"
        />

        <FieldVerifier 
            :isValid="!unexpectedError"
            :withErrorIcon="showErrorIcon"
            :hideWhenNoError="true"
            description="Ein unerwarteter Fehler ist aufgetreten."
        />

        <div class="button-container">
            <button class="filled-button" @click="submitFrom">Anmelden</button>
        </div>
    </div>
</template>
