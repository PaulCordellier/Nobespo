<script setup lang="ts">
import { useTemplateRef } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const { searchRouteName } = defineProps<{
    searchRouteName: string
}>()

const router = useRouter()
const route = useRoute()
const textField = useTemplateRef('search-bar')

function onEnterPressed() {
    let writtenText = textField.value!.value

    if (!textField || writtenText.length <= 0) {
        return
    }
    
    let query = encodeURI(writtenText)

    router.push({ name: searchRouteName, params: { query }})
}
</script>

<template>
    <input
        placeholder="Films oder Series suchen"
        id="search-bar"
        ref="search-bar"
        type="search"
        :value="route.params.query ? decodeURI(route.params.query as string) : ''"
        @keyup.enter="onEnterPressed"
    >
</template>