<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'

export type FieldVerifierInfo = {
    isValid: () => boolean,
    description: string
}

const { isValid, showErrorIcon, hideWhenNoError, description } = defineProps<{
    isValid: boolean,
    showErrorIcon: boolean,
    hideWhenNoError: boolean,
    description: string
}>()

const noErrorImagePath = "images/mini-icons/accepted.png"
const errorImagePath = "images/mini-icons/error.png"
const noSignalImagePath = "images/mini-icons/circle.svg"

onMounted(setBulletImagePath)

const bulletImagePath = ref<string>()
const showCompoent = ref<boolean>()

watch(() => showErrorIcon, setBulletImagePath)
watch(() => isValid, setBulletImagePath)

function setBulletImagePath() {

    if (hideWhenNoError) {

        if (showErrorIcon && !isValid) {
            bulletImagePath.value = errorImagePath
            showCompoent.value = true
        } else if (showErrorIcon) {
            showCompoent.value = false
        }

    } else {

        showCompoent.value = true
        
        if (isValid) {
            bulletImagePath.value = noErrorImagePath
        } else if (showErrorIcon) {
            bulletImagePath.value = errorImagePath
        } else {
            bulletImagePath.value = noSignalImagePath
        }

    }
}
</script>


<template>
    <div class="field-verifier" v-show="showCompoent">
        <img :src="bulletImagePath">
        <p>{{ description }}</p>
    </div>
</template>


<style lang="scss">
.field-verifier {
    display: flex;
    column-gap: 10px;
    align-items: center;
    padding: 5px 0;
}

.field-verifier img {
    flex-grow: 0;
    flex-shrink: 0;
    width: 25px;
    height: 25px;
}
</style>