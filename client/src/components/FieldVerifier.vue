<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import noErrorImagePath from "@/assets/images/icons/accepted.png"
import errorImagePath from "@/assets/images/icons/error.png"
import noSignalImagePath from "@/assets/images/icons/info.svg"

export type FieldVerifierInfo = {
    isValid: () => boolean,
    description: string
}

const { isValid, withErrorIcon, hideWhenNoError, description } = defineProps<{
    isValid: boolean,
    withErrorIcon: boolean,
    hideWhenNoError: boolean,
    description: string
}>()


onMounted(setBulletImagePath)

const bulletImagePath = ref<string>()
const showCompoent = ref<boolean>()

watch(() => withErrorIcon, setBulletImagePath)
watch(() => isValid, setBulletImagePath)

function setBulletImagePath() {

    if (hideWhenNoError) {

        if (withErrorIcon && !isValid) {
            bulletImagePath.value = errorImagePath
            showCompoent.value = true
        } else if (withErrorIcon) {
            showCompoent.value = false
        }

    } else {

        showCompoent.value = true
        
        if (isValid) {
            bulletImagePath.value = noErrorImagePath
        } else if (withErrorIcon) {
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