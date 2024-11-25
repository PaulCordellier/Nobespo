<script setup lang="ts">
import { MdOutlinedTextField } from '@material/web/textfield/outlined-text-field';
import { MdFilledButton } from '@material/web/button/filled-button';
import FieldVerifier from "@/components/FieldVerifier.vue";
import { type FieldVerifierInfo } from "@/components/FieldVerifier.vue";
import { ref, type Ref, watch } from 'vue';

const password = ref<string>("");

watch(password, verifyAllFields)

const allFieldsAreValid : Ref<Boolean, Boolean> = ref<Boolean>(false)
const showErrorIcon : Ref<Boolean, Boolean> = ref<Boolean>(false)

let passwordVerifierInfos : FieldVerifierInfo[] = [
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

function verifyAllFields() {

    allFieldsAreValid.value = true

    for (const passwordVerifierInfo of passwordVerifierInfos) {
        if (!passwordVerifierInfo.isValid()) {
            allFieldsAreValid.value = false
            break
        }
    }
}
</script>

<template>
    <div id="connexion-block">
        <h1>Welcome!</h1>
        <md-outlined-text-field type="email" label="E-mail"></md-outlined-text-field>
        <md-outlined-text-field type="text" label="Username"></md-outlined-text-field>
        <md-outlined-text-field type="password" label="Password" v-model="password"></md-outlined-text-field>
        
        <div v-for="passwordVerifierInfo in passwordVerifierInfos">
            <FieldVerifier 
                :isValid="passwordVerifierInfo.isValid()"
                :showErrorIcon="showErrorIcon"
            >
                {{ passwordVerifierInfo.description }}
            </FieldVerifier>
        </div>

        <md-outlined-text-field type="password" label="Password again"></md-outlined-text-field>
        <div class="button-container">
            <md-filled-button :disabled="!allFieldsAreValid">Sign up</md-filled-button>
        </div>
    </div>
</template>
