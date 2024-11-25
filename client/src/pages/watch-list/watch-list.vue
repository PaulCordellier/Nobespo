<script setup lang="ts">
  import { MdDialog } from '@material/web/dialog/dialog';
  import '@material/web/button/outlined-button.js';

  import WriteCommentDialog from '@/components/WriteCommentDialog.vue'

  function onWriteCommentsButtonClick() {
    let dialogWriteComment: MdDialog = document.getElementById("write-comment-dialog") as MdDialog
    dialogWriteComment.show();
  }
</script>

<template>
  <div id="film-serie-watchlist-full-description">
    <div>
      <h1>Best of Post-apo</h1>
      <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident.</p>

      <p class="stars" style="font-size: 1.5em;">★★★★★</p>
      <md-outlined-button>Erstellen</md-outlined-button>
      <md-outlined-button @click="onWriteCommentsButtonClick()">Beschreibung schreiben</md-outlined-button>
    </div>
  </div>

  <div id="film-or-series">
    <a class="film-or-serie" href="/movie-serie">
      <img src="https://placehold.co/100x150" />
      <div>
        <h2>Mad Max: Fury Road</h2>
        <p>In Ixtenco, Mexico, Nick Fury and Maria Hill investigate an unnatural storm and encounter the Earth Elemental. Quentin Beck, a super-powered individual, arrives and defeats the creature. He is subsequently recruited by Fury and Hill.</p>
      </div>
    </a>
    <a class="film-or-serie" href="/movie-serie">
      <img src="https://placehold.co/100x150" />
      <div>
        <h2>Stalker</h2>
        <p>After Quentin Beck frames Peter Parker for his murder and reveals that Peter is Spider-Man, the Department of Damage Control interrogates Peter; his girlfriend, Michelle "MJ" Jones-Watson; his best friend, Ned Leeds; and his aunt, May Parker. Lawyer Matt Murdock gets Peter's charges dropped, but the group grapples with negative publicity.</p>
      </div>
    </a>
    <a class="film-or-serie" href="/movie-serie">
      <img src="https://placehold.co/100x150" />
      <div>
        <h2>Snowpiercer</h2>
        <p>After Quentin Beck frames Peter Parker for his murder and reveals that Peter is Spider-Man, the Department of Damage Control interrogates Peter; his girlfriend, Michelle "MJ" Jones-Watson; his best friend, Ned Leeds; and his aunt, May Parker. Lawyer Matt Murdock gets Peter's charges dropped, but the group grapples with negative publicity.</p>
      </div>
    </a>
  </div>

  <div id="comments">
    <h1>Kommentar</h1>
    <div class="comment">
      <div class="info-line-comment">
        <img src="https://s.ltrbxd.com/static/img/avatar48.4cc5919f.png" />
        <p>user415642</p>
        <p class="stars">★★★</p>
      </div>
      <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident.</p>
    </div>
    <div class="comment">
      <div class="info-line-comment">
        <img src="https://s.ltrbxd.com/static/img/avatar48.4cc5919f.png" />
        <p>highiquser</p>
        <p class="stars">★★★★</p>
      </div>
      <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident.</p>
    </div>
  </div>

  <WriteCommentDialog />
</template>
