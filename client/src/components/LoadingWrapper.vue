<script setup lang="ts">
import { dotSpinner } from 'ldrs'

dotSpinner.register()

defineProps<{
    loadedRef: any,
    errorMessage: string | undefined
}>()
</script>

<template>

<p v-if="!loadedRef && errorMessage" class="loadingErrorMessage">{{ errorMessage }}</p>
<slot v-else-if="loadedRef"></slot>
<div v-else class="loader-wrapper">
    <l-dot-spinner
        class="spinner"
        size="40"
        speed="0.9" 
        color="white" />
</div>

</template>

<style lang="scss">
.loader-wrapper {

    position: relative;
    display: block;

    .spinner {
        position: absolute;
        left: 50%;
        transform: translateX(-50%);
        margin: 15px auto;
    }
}
</style>