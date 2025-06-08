<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

const modulos = ref([
  {
    id: 'produtos',
    titulo: 'Gerenciar Produtos',
    descricao: 'Adicione, edite ou remova produtos da loja',
    icone: '📦',
    rota: '/admin/produtos'
  },
  {
    id: 'categorias',
    titulo: 'Gerenciar Categorias',
    descricao: 'Organize suas categorias de produtos',
    icone: '🏷️',
    rota: '/admin/categorias'
  },
  {
    id: 'estoque',
    titulo: 'Controle de Estoque',
    descricao: 'Atualize quantidades e status de estoque',
    icone: '📊',
    rota: '/admin/estoque'
  }
])

const navegarPara = (rota: string) => {
  router.push(rota)
}
</script>

<template>
  <div class="admin">
    <div class="container">
      <header class="admin__cabecalho">
        <h1 class="admin__titulo">Painel Administrativo</h1>
        <router-link to="/" class="admin__voltar">
          Voltar para loja
        </router-link>
      </header>
      
      <div class="admin__conteudo">
        <div class="admin__grade">
          <div 
            v-for="modulo in modulos" 
            :key="modulo.id"
            class="admin__card"
            @click="navegarPara(modulo.rota)"
          >
            <div class="admin__card-icone">{{ modulo.icone }}</div>
            <h2 class="admin__card-titulo">{{ modulo.titulo }}</h2>
            <p class="admin__card-descricao">{{ modulo.descricao }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.admin {
  &__cabecalho {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: var(--espacamento-grande);
  }
  
  &__titulo {
    font-size: var(--fonte-tamanho-grande);
  }
  
  &__voltar {
    display: inline-flex;
    align-items: center;
    padding: var(--espacamento-pequeno) var(--espacamento-medio);
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
    transition: var(--transicao-padrao);
    
    &:hover {
      background-color: var(--cor-cinza-claro);
    }
  }
  
  &__conteudo {
    margin-bottom: var(--espacamento-extra-grande);
  }
  
  &__grade {
    display: grid;
    grid-template-columns: repeat(1, 1fr);
    gap: var(--espacamento-grande);
    
    @media (min-width: 576px) {
      grid-template-columns: repeat(2, 1fr);
    }
    
    @media (min-width: 992px) {
      grid-template-columns: repeat(3, 1fr);
    }
  }
  
  &__card {
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
    padding: var(--espacamento-grande);
    box-shadow: var(--sombra-pequena);
    transition: var(--transicao-padrao);
    cursor: pointer;
    
    &:hover {
      transform: translateY(-5px);
      box-shadow: var(--sombra-media);
    }
    
    &-icone {
      font-size: 3rem;
      margin-bottom: var(--espacamento-medio);
    }
    
    &-titulo {
      font-size: var(--fonte-tamanho-medio);
      margin-bottom: var(--espacamento-pequeno);
    }
    
    &-descricao {
      color: var(--cor-cinza-escuro);
    }
  }
}
</style>