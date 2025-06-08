<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'

const props = defineProps<{
  posicaoAbsoluta?: boolean
}>()

const emit = defineEmits(['fechar'])
const router = useRouter()

const categorias = ref([
  { id: 1, nome: 'Suplementos', slug: 'suplementos' },
  { id: 2, nome: 'Acessórios', slug: 'acessorios' },
  { id: 3, nome: 'Roupas Fitness', slug: 'roupas' },
  { id: 4, nome: 'Equipamentos', slug: 'equipamentos' },
  { id: 5, nome: 'Nutrição', slug: 'nutricao' }
])

const handleClickFora = (event: MouseEvent) => {
  const target = event.target as HTMLElement
  if (!target.closest('.categoria-menu')) {
    emit('fechar')
  }
}

const navegar = (slug: string) => {
  router.push({
    path: '/busca',
    query: { categoria: slug }
  })
  emit('fechar')
}

onMounted(() => {
  document.addEventListener('click', handleClickFora)
})

onUnmounted(() => {
  document.removeEventListener('click', handleClickFora)
})
</script>

<template>
  <div class="categoria-menu" :class="{ 'categoria-menu--absoluta': posicaoAbsoluta }">
    <ul class="categoria-menu__lista">
      <li 
        v-for="categoria in categorias" 
        :key="categoria.id" 
        class="categoria-menu__item"
        @click="navegar(categoria.slug)"
      >
        {{ categoria.nome }}
      </li>
    </ul>
  </div>
</template>

<style lang="scss" scoped>
.categoria-menu {
  background-color: var(--cor-branco);
  border-radius: var(--borda-raio);
  box-shadow: var(--sombra-media);
  animation: fadeIn 0.2s ease-in-out;
  
  &--absoluta {
    position: absolute;
    top: 100%;
    left: 0;
    width: 200px;
    z-index: 1000;
  }
  
  &__lista {
    padding: var(--espacamento-pequeno) 0;
  }
  
  &__item {
    padding: var(--espacamento-pequeno) var(--espacamento-medio);
    cursor: pointer;
    transition: var(--transicao-padrao);
    
    &:hover {
      background-color: var(--cor-cinza-claro);
      color: var(--cor-primaria);
    }
  }
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>