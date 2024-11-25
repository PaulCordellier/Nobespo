import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    // route level code-splitting
    // this generates a separate chunk (About.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import('@/pages/home-page.vue')
  },
  {
    path: '/log-in',
    component: () => import('@/pages/connexion/log-in.vue')
  },
  {
    path: '/sign-up',
    component: () => import('@/pages/connexion/sign-up.vue')
  },
  {
    path: '/films-series',
    component: () => import('@/pages/film-serie/films-and-series.vue')
  },
  {
    path: '/films-series/search/:query',
    name: 'search-films-series',
    component: () => import('@/pages/film-serie/search-films-and-series.vue')
  },
  {
    path: '/films-series/film/:id',
    name: 'film',
    component: () => import('@/pages/film-serie/film.vue')
  },
  {
    path: '/films-series/serie/:id',
    name: 'serie',
    component: () => import('@/pages/film-serie/serie.vue')
  },
  {
    path: '/watch-lists',
    component: () => import('@/pages/watch-list/watch-lists.vue')
  },
  {
    path: '/watch-lists/search/:query',
    name: 'search-watch-lists',
    component: () => import('@/pages/watch-list/search-watch-lists.vue')
  },
];

export default createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
});
