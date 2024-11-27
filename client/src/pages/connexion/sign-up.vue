<script setup lang="ts">
import { MdFilledButton } from '@material/web/button/filled-button';
import FieldVerifier from "@/components/FieldVerifier.vue";
import { type FieldVerifierInfo } from "@/components/FieldVerifier.vue";
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { type Account } from "@/models/account";
import { useCurrentUserStore } from "@/stores/currentUser";

const router = useRouter()
const currentUserStore = useCurrentUserStore()

const username = ref<string>("");
const password = ref<string>("");
const passwordAgain = ref<string>("");

const showErrorIcon = ref<boolean>(false)

const usernameVerifierInfos :  FieldVerifierInfo[] = [
    {
        isValid: () => {
            username.value.replace(/\s+/g, "")
            return (username.value != "")
        },
        description: "Das Feld Benutzername ist obligatorisch."
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

async function submitFrom() {

    if (!allFieldsAreValid()) {
        showErrorIcon.value = true
        return
    }

    const account = {
        "username": username.value,
        "password": password.value
    }

    const response = await fetch("/api/account/signup", {
        method: "POST",
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(account)
    })

    if (response.ok) {
        const currentUserData : any = await response.json()
        const currentAccount : Account = currentUserData.account

        currentUserStore.saveUserData(currentAccount, currentUserData.token)

        router.push('/')
    } else {
        // TODO afficher l'erreur ici
    }

}
</script>   

<template>
    <div id="connexion-block">
        <h1>Wilkommen!</h1>

        <label for="username" class="text-field-label">Benutzername:</label>
        <input type="text" v-model="username" id="username" class="text-field">
        <div v-for="usernameVerifierInfo in usernameVerifierInfos" v-bind:key="usernameVerifierInfo.description">
            <FieldVerifier 
                :isValid="usernameVerifierInfo.isValid()"
                :showErrorIcon="showErrorIcon"
                :hideWhenNoError="true"
                :description="usernameVerifierInfo.description"
            />
        </div>

        <label for="password" class="text-field-label">Passwort:</label>
        <input type="password" v-model="password" id="password" class="text-field">
        <div v-for="passwordVerifierInfo in passwordVerifierInfos" v-bind:key="passwordVerifierInfo.description">
            <FieldVerifier 
                :isValid="passwordVerifierInfo.isValid()"
                :showErrorIcon="showErrorIcon"
                :hideWhenNoError="false"
                :description="passwordVerifierInfo.description"
            />
        </div>

        <label for="password-again" class="text-field-label">Passwort überprüfen:</label>
        <input type="password" v-model="passwordAgain" id="password-again" class="text-field">

        <FieldVerifier 
            :isValid="verifyTwoPasswords.isValid()"
            :showErrorIcon="showErrorIcon"
            :hideWhenNoError="true"
            :description="verifyTwoPasswords.description"
        />

        <div class="button-container">
            <md-filled-button @click="submitFrom">Anmelden</md-filled-button>
        </div>
    </div>
</template>
