<script setup lang="ts">
import Cabecalho from './components/layout/Cabecalho.vue'
import Rodape from './components/layout/Rodape.vue'
</script>

<template>
  <div class="app">
    <Cabecalho />
    <main class="app__conteudo">
      <router-view v-slot="{ Component }">
        <transition name="pagina" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </main>
    <Rodape />
  </div>
</template>

<style lang="scss">
.pagina-enter-active,
.pagina-leave-active {
  transition: opacity 0.3s ease;
}

.pagina-enter-from,
.pagina-leave-to {
  opacity: 0;
}
</style>