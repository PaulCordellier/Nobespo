<script setup lang="ts">
  import '@material/web/textfield/outlined-text-field.js';
  //import '@material/web/icon/icon.js';
</script>

<template>
  <md-outlined-text-field placeholder="Watchlists suchen" id="search-bar">
    <!--<md-icon slot="leading-icon">search</md-icon>-->
  </md-outlined-text-field>

  <div id="search-results">
    <a class="film-or-serie" href="/watch-lists">
      <img src="https://placehold.co/100x150" />
      <div>
        <h2>Best of Post-apo</h2>
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident.</p>
      </div>
    </a>
    <a class="film-or-serie" href="/watch-lists">
      <img src="https://placehold.co/100x150" />
      <div>
        <h2>Best spider man movies</h2>
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident.</p>
      </div>
    </a>
  </div>
</template>
