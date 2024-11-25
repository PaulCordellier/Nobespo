<script setup lang="ts">
import { onMounted, ref, watch, defineProps } from 'vue'

export type FieldVerifierInfo = {
    isValid: () => Boolean,
    description: string
}

const { isValid, showErrorIcon } = defineProps<{
    isValid: Boolean,
    showErrorIcon: Boolean
}>()

const noErrorImagePath = "images/mini-icons/accepted.png"
const errorImagePath = "images/mini-icons/error.png"
const noSignalImagePath = "images/mini-icons/circle.svg"

onMounted(setBulletImagePath)

const bulletImagePath = ref<string>()

watch(() => showErrorIcon, setBulletImagePath)
watch(() => isValid, setBulletImagePath)

function setBulletImagePath() {
    if (isValid){
        bulletImagePath.value = noErrorImagePath
    } else if (showErrorIcon) {
        bulletImagePath.value = errorImagePath
    } else {
        bulletImagePath.value = noSignalImagePath
    }
}
</script>


<template>
    <div class="field-verifier">
        <img :src="bulletImagePath" alt="">
        <p><slot></slot></p>
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