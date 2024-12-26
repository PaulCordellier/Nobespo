<script setup lang="ts">
import { dotSpinner } from 'ldrs'

dotSpinner.register()

defineProps<{
    buttonText: string
    responseState: ResponseState
    buttonEvent: (event: Event) => void
}>()
</script>

<script lang="ts">
export enum ResponseState {
    NoRequest,
    Loading,
    Error
}
</script>

<template>
    <button
        v-if="responseState != ResponseState.Loading"
        class="primary-button"
        id="add-button"
        @click="buttonEvent">
        {{ buttonText }}
    </button>

    <l-dot-spinner
        v-if="responseState == ResponseState.Loading"
        class="spinner"
        size="40"
        speed="0.9" 
        color="white" />
    
    <p v-if="responseState == ResponseState.Error" class="loadingErrorMessage">Fehler bei der Verbindung zum Server.</p>
</template>