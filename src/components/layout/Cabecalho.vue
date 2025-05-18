<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useCarrinhoStore } from '../../stores/carrinho'
import CategoriaMenu from '../ui/CategoriaMenu.vue'

const router = useRouter()
const carrinhoStore = useCarrinhoStore()
const termoBusca = ref('')
const menuCategoriaAberto = ref(false)

const buscar = () => {
  if (termoBusca.value.trim()) {
    router.push({
      path: '/busca',
      query: { termo: termoBusca.value }
    })
  }
}

const toggleMenuCategoria = () => {
  menuCategoriaAberto.value = !menuCategoriaAberto.value
}
</script>

<template>
  <header class="cabecalho">
    <div class="container">
      <div class="cabecalho__conteudo">
        <router-link to="/" class="cabecalho__logo">
          <img src="/src/assets/logoFitShop.svg" class="cabecalho__logo__logo" alt="logo loja fitshop">
          <h1 class="cabecalho__titulo">FitShop</h1>
        </router-link>
        
        <div class="cabecalho__busca">
          <form @submit.prevent="buscar" class="busca">
            <input 
              type="text" 
              v-model="termoBusca" 
              class="busca__input" 
              placeholder="O que você procura?"
            />
            <button type="submit" class="busca__botao">
              <span class="busca__botao-texto">Buscar</span>
            </button>
          </form>
        </div>
        
        <div class="cabecalho__acoes">
          <router-link to="/carrinho" class="cabecalho__carrinho">
            <span class="cabecalho__carrinho-icone">🛒</span>
            <span v-if="carrinhoStore.quantidadeTotal" class="cabecalho__carrinho-badge">
              {{ carrinhoStore.quantidadeTotal }}
            </span>
          </router-link>
          
          <router-link to="/admin" class="cabecalho__admin">
            <span class="cabecalho__admin-texto">Admin</span>
          </router-link>
        </div>
      </div>
    </div>
  </header>
</template>

<style lang="scss" scoped>
.cabecalho {
  background-color: var(--cor-primaria);
  color: var(--cor-branco);
  padding: var(--espacamento-medio) 0;
  position: sticky;
  top: 0;
  z-index: 1000;
  box-shadow: var(--sombra-pequena);

  .cabecalho__logo__logo{
    display: flex;
    align-items: center;
    width: 40%;
  }
  
  &__conteudo {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: var(--espacamento-medio);
    
    @media (min-width: 992px) {
      flex-direction: row;
      justify-content: space-between;
    }
  }
  
  &__logo {
    color: var(--cor-branco);
    display: flex;
    align-items: center;
    transition: var(--transicao-padrao);
    
    &:hover {
      transform: scale(1.05);
      color: var(--cor-branco);
    }
  }
  
  &__titulo {
    font-size: var(--fonte-tamanho-grande);
    font-weight: 700;
  }
  
  &__busca {
    flex: 1;
    width: 100%;
    max-width: 600px;
    
    @media (min-width: 992px) {
      margin: 0 var(--espacamento-grande);
    }
  }
  
  &__acoes {
    display: flex;
    align-items: center;
    gap: var(--espacamento-medio);
  }
  
  &__carrinho {
    position: relative;
    color: var(--cor-branco);
    font-size: var(--fonte-tamanho-grande);
    transition: var(--transicao-padrao);
    
    &:hover {
      color: var(--cor-branco);
      transform: scale(1.1);
    }
    
    &-badge {
      position: absolute;
      top: -8px;
      right: -8px;
      background-color: var(--cor-secundaria);
      color: var(--cor-branco);
      font-size: var(--fonte-tamanho-pequeno);
      font-weight: 700;
      width: 20px;
      height: 20px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
    }
  }
  
  &__admin {
    display: flex;
    align-items: center;
    gap: var(--espacamento-pequeno);
    color: var(--cor-branco);
    padding: var(--espacamento-pequeno) var(--espacamento-medio);
    border-radius: var(--borda-raio);
    background-color: rgba(255, 255, 255, 0.1);
    transition: var(--transicao-padrao);
    
    &:hover {
      background-color: rgba(255, 255, 255, 0.2);
      color: var(--cor-branco);
    }
    
    &-texto {
      @media (max-width: 576px) {
        display: none;
      }
    }
  }
}

.busca {
  display: flex;
  position: relative;
  
  &__input {
    flex: 1;
    padding: var(--espacamento-pequeno) var(--espacamento-medio);
    border: none;
    border-radius: var(--borda-raio) 0 0 var(--borda-raio);
    font-size: var(--fonte-tamanho-base);
    
    &:focus {
      outline: none;
    }
  }
  
  &__categoria {
    position: relative;
    
    &-botao {
      height: 100%;
      display: flex;
      align-items: center;
      gap: var(--espacamento-pequeno);
      padding: 0 var(--espacamento-medio);
      background-color: var(--cor-cinza-claro);
      border: none;
      font-size: var(--fonte-tamanho-base);
      cursor: pointer;
      
      &:hover {
        background-color: var(--cor-cinza);
      }
    }
    
    &-icone {
      transition: transform 0.3s ease;
      
      &--aberto {
        transform: rotate(180deg);
      }
    }
    
    &-menu {
      position: absolute;
      top: 100%;
      left: 0;
      right: 0;
      z-index: 10;
    }
  }
  
  &__botao {
    padding: var(--espacamento-pequeno) var(--espacamento-medio);
    background-color: var(--cor-secundaria);
    color: var(--cor-branco);
    border: none;
    border-radius: 0 var(--borda-raio) var(--borda-raio) 0;
    cursor: pointer;
    transition: var(--transicao-padrao);
    
    &:hover {
      background-color: var(--cor-secundaria-escura);
    }
    
    &-texto {
      @media (max-width: 576px) {
        display: none;
      }
    }
  }
}
</style>