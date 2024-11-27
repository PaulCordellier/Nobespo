<script setup lang="ts">
import { MdDialog } from '@material/web/dialog/dialog';
import { MdOutlinedButton } from '@material/web/button/outlined-button';

import WriteCommentDialog from '@/components/WriteCommentDialog.vue';
import CommentSection from "@/components/CommentSection.vue";

import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";

const route = useRoute()

const media = ref()

onMounted(fetchTrending)

async function fetchTrending() {
    
    const response = await fetch(`/api/tmdb/serie/${route.params.id}`, { method: "GET" })

    if (response.ok) {
        media.value = await response.json()
    }
}

function onWriteCommentsButtonClick() {
    const dialogWriteComment: MdDialog = document.getElementById("write-comment-dialog") as MdDialog
    dialogWriteComment.show();
}
</script>

<template>
    <div v-if="media" class="basic-padding-container">
        <WriteCommentDialog />
        <div id="media-serie-watchlist-full-description">   <!-- todo vérifier le nom de cette classe -->
            <img :src="`https://image.tmdb.org/t/p/w154${media.poster_path}`"/>
            <div>
                <h1>{{ media.name }}</h1>
                <p>{{ media.overview }}</p>

                <p class="stars" style="font-size: 1.5em;">★★★★★</p>
                <md-outlined-button>Erstellen</md-outlined-button>
                <md-outlined-button @click="onWriteCommentsButtonClick()">Beschreibung schreiben</md-outlined-button>
            </div>
        </div>
        <CommentSection />
    </div>
</template>
