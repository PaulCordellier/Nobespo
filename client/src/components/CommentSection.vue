<script setup lang="ts">
import { type Comment } from '@/models/comment'
import { onMounted, ref, useTemplateRef } from 'vue'

import { useCurrentUserStore } from "@/stores/currentUser"

import { MdFilledButton } from '@material/web/button/filled-button'

const currentUserStore = useCurrentUserStore()

const commentTextArea = useTemplateRef("comment-text-area")

const { urlToGetComments, urlToPublishComments } = defineProps<{
    urlToGetComments: string
    urlToPublishComments: string
}>()

const comments = ref<Comment[] | null>(null);

onMounted(getComments)

async function getComments() {
    const response = await fetch(urlToGetComments, {method: 'GET'})

    if (response.ok) {
        comments.value = await response.json() as Comment[]
    } else {
        // TODO
    }
}

const showPostButton = ref<boolean>(false)

function onCommentTextAreaInput() {
    commentTextArea.value!.style.height = "50px"
    commentTextArea.value!.style.height = (commentTextArea.value!.scrollHeight + 5) + "px"
    showPostButton.value = commentTextArea.value?.value != ''
}

async function publishComment() {
    const options = {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${currentUserStore.token}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(commentTextArea.value?.value)
    };

    const response = await fetch(urlToPublishComments, options)

    if (response.ok) {
        console.log("yay")
        await getComments()
    }
}
</script>

<template>
    <div id="comments">
        <h1>Kommentare:</h1>
        <div class="comment">
            <div class="info-line-comment">
                <img src="@/assets/images/icons/default-user.png" />
                <p>{{ currentUserStore.isConnected ? currentUserStore.username : "user name here" }}</p>
            </div>
            <textarea
                id="comment-text-area"
                class="text-field"
                ref="comment-text-area"
                placeholder="Schreiben Sie Ihren Kommentar hier!"
                maxlength="20000"
                contenteditable="true"
                @input="onCommentTextAreaInput" />
            <md-filled-button v-if="showPostButton" @click="publishComment" :disabled="!currentUserStore.isConnected ">
                Posten
            </md-filled-button>
        </div>
        <div class="comment" v-if="comments && comments.length >= 1" v-for="comment in comments">
            <div class="info-line-comment">
                <img src="@/assets/images/icons/default-user.png" />
                <p>{{ comment.username }}</p>
                <p class="stars">★★★★</p>
            </div>
            <p>{{ comment.text }}</p>
        </div>
        <div class="comment" v-if="comments && comments.length == 0">
            <p>Es gibt noch keine Kommentare.</p>
        </div>
    </div>
</template>


<style lang="scss">
#comments {
	padding-top: 15px;
}

.comment {
	padding: 10px 0;
}

.info-line-comment {
	display: flex;
	gap: 10px;
	-webkit-align-items: center;
	align-items: center;
    
	img {
		-moz-border-radius: 50%;
		-webkit-border-radius: 50%;
		border-radius: 50%;
        width: 50px;
        height: 50px;
	}
}

#comment-text-area {
    margin-top: 10px;
    resize: none;
    font: inherit;
    height: 50px;
}

.stars {
	color: green;
}
</style>