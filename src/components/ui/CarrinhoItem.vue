<script setup lang="ts">
import { computed } from 'vue'
import { useCarrinhoStore } from '../../stores/carrinho'

const props = defineProps<{
  item: {
    id: number
    nome: string
    preco: number
    imagem: string
    quantidade: number
  }
}>()

const carrinhoStore = useCarrinhoStore()

const formatarPreco = (valor: number) => {
  return valor.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  })
}

const precoTotal = computed(() => formatarPreco(props.item.preco * props.item.quantidade))

const aumentarQuantidade = () => {
  carrinhoStore.atualizarQuantidade(props.item.id, props.item.quantidade + 1)
}

const diminuirQuantidade = () => {
  if (props.item.quantidade > 1) {
    carrinhoStore.atualizarQuantidade(props.item.id, props.item.quantidade - 1)
  }
}

const removerItem = () => {
  carrinhoStore.removerItem(props.item.id)
}
</script>

<template>
  <div class="carrinho-item">
    <div class="carrinho-item__imagem-container">
      <img :src="item.imagem" :alt="item.nome" class="carrinho-item__imagem" />
    </div>
    
    <div class="carrinho-item__info">
      <h3 class="carrinho-item__nome">{{ item.nome }}</h3>
      <div class="carrinho-item__preco">
      </div>
    </div>
    
    <div class="carrinho-item__quantidade">
      <button 
        class="carrinho-item__quantidade-botao" 
        @click="diminuirQuantidade"
        :disabled="item.quantidade <= 1"
      >
        -
      </button>
      <span class="carrinho-item__quantidade-valor">{{ item.quantidade }}</span>
      <button class="carrinho-item__quantidade-botao" @click="aumentarQuantidade">
        +
      </button>
    </div>
    
    <div class="carrinho-item__total">
      {{ precoTotal }}
    </div>
    
    <button class="carrinho-item__remover" @click="removerItem">
      ×
    </button>
  </div>
</template>

<style lang="scss" scoped>
.carrinho-item {
  display: grid;
  grid-template-columns: 80px 1fr auto auto 40px;
  gap: var(--espacamento-medio);
  align-items: center;
  padding: var(--espacamento-medio);
  background-color: var(--cor-branco);
  border-radius: var(--borda-raio);
  margin-bottom: var(--espacamento-medio);
  box-shadow: var(--sombra-pequena);
  
  @media (max-width: 768px) {
    grid-template-columns: 60px 1fr;
    grid-template-rows: auto auto auto;
  }
  
  &__imagem-container {
    width: 80px;
    height: 80px;
    overflow: hidden;
    border-radius: var(--borda-raio-pequeno);
    background-color: var(--cor-cinza-claro);
    
    @media (max-width: 768px) {
      width: 60px;
      height: 60px;
      grid-row: span 2;
    }
  }
  
  &__imagem {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
  
  &__info {
    @media (max-width: 768px) {
      grid-column: 2;
    }
  }
  
  &__nome {
    font-size: var(--fonte-tamanho-medio);
    margin-bottom: var(--espacamento-pequeno);
  }
  
  &__preco {
    color: var(--cor-cinza-escuro);
  }
  
  &__quantidade {
    display: flex;
    align-items: center;
    gap: var(--espacamento-pequeno);
    
    @media (max-width: 768px) {
      grid-column: 2;
      grid-row: 2;
    }
  }
  
  &__quantidade-botao {
    width: 30px;
    height: 30px;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: var(--cor-cinza-claro);
    border: none;
    border-radius: var(--borda-raio-pequeno);
    font-weight: bold;
    cursor: pointer;
    transition: var(--transicao-padrao);
    
    &:hover {
      background-color: var(--cor-cinza);
    }
    
    &:disabled {
      opacity: 0.5;
      cursor: not-allowed;
    }
  }
  
  &__quantidade-valor {
    width: 30px;
    text-align: center;
    font-weight: 500;
  }
  
  &__total {
    font-weight: 700;
    color: var(--cor-primaria);
    
    @media (max-width: 768px) {
      grid-column: 1 / -1;
      grid-row: 3;
      text-align: right;
    }
  }
  
  &__remover {
    background: none;
    border: none;
    color: var(--cor-erro);
    font-size: 24px;
    cursor: pointer;
    transition: var(--transicao-padrao);
    display: flex;
    align-items: center;
    justify-content: center;
    width: 30px;
    height: 30px;
    
    &:hover {
      transform: scale(1.2);
    }
    
    @media (max-width: 768px) {
      position: absolute;
      top: var(--espacamento-pequeno);
      right: var(--espacamento-pequeno);
    }
  }
}
</style>