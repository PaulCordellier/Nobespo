<script setup lang="ts">
import { ref } from 'vue';
import { MdFilledButton } from '@material/web/button/filled-button';

const username = ref<string>("");
const password = ref<string>("");

async function submitFrom() {

    const account = {
        "username": username.value,
        "password": password.value
    }

    const response = await fetch("/api/account/login", {
        method: "POST",
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(account)
    })

    if (response.ok) {
        // TODO
    } else {
        // TODO
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

        <div class="button-container">
            <md-filled-button @click="submitFrom">Einloggen</md-filled-button>
        </div>
    </div>
</template>
