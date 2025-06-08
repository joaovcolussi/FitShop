<script setup lang="ts">
import { computed } from 'vue'
import { useCarrinhoStore } from '../../stores/carrinho'

const props = defineProps<{
  produto?: {
    id: number
    nome: string
    preco: number
    precoPromocional?: number
    promocao?: boolean
    imagem: string
    categoria: string
    estoque: number
  }
}>()

const carrinhoStore = useCarrinhoStore()

const formatarPreco = (valor: number) => {
  return valor.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  })
}

// Mostrar preço promocional se aplicável
const precoFinal = computed(() => {
  if (props.produto?.promocao && props.produto?.precoPromocional) {
    return props.produto.precoPromocional
  }
  return props.produto?.preco ?? 0
})

const precoFormatado = computed(() => formatarPreco(precoFinal.value))

const adicionarAoCarrinho = () => {
  carrinhoStore.adicionarItem({
    ...props.produto,
    preco: precoFinal.value,
    quantidade: 1
  })
}
</script>

<template>
  <div v-if="produto" class="produto-card">
    <router-link :to="`/produto/${produto.id}`" class="produto-card__link">
      <div class="produto-card__imagem-container">
        <img :src="produto.imagem" :alt="produto.nome" class="produto-card__imagem" />
      </div>
      <div class="produto-card__info">
        <span class="produto-card__categoria">{{ produto.categoria }}</span>
        <h3 class="produto-card__nome">{{ produto.nome }}</h3>
        <div class="produto-card__preco">
          <template v-if="produto.promocao && produto.precoPromocional">
            <span class="preco-original">{{ formatarPreco(produto.preco) }}</span>
            <span class="preco-promocional">{{ precoFormatado }}</span>
          </template>
          <template v-else>
            {{ precoFormatado }}
          </template>
        </div>
      </div>
    </router-link>
    <div class="produto-card__acoes">
      <button 
        class="botao produto-card__botao" 
        @click="adicionarAoCarrinho"
        :disabled="produto.estoque === 0"
      >
        <span v-if="produto.estoque > 0">Adicionar ao carrinho</span>
        <span v-else>Fora de estoque</span>
      </button>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.produto-card {
  background-color: var(--cor-branco);
  border-radius: var(--borda-raio);
  overflow: hidden;
  box-shadow: var(--sombra-pequena);
  transition: var(--transicao-padrao);
  display: flex;
  flex-direction: column;
  height: 100%;
  
  &:hover {
    box-shadow: var(--sombra-media);
    transform: translateY(-5px);
    
    .produto-card__imagem {
      transform: scale(1.05);
    }
  }

  &__link {
    color: inherit;
    text-decoration: none;
    display: flex;
    flex-direction: column;
    flex: 1;
  }

  &__imagem-container {
    height: 200px;
    overflow: hidden;
    background-color: var(--cor-cinza-claro);
  }

  &__imagem {
    width: 100%;
    height: 100%;
    object-fit: cover;
    transition: transform 0.3s ease;
  }

  &__info {
    padding: var(--espacamento-medio);
    flex: 1;
    display: flex;
    flex-direction: column;
  }

  &__categoria {
    font-size: var(--fonte-tamanho-pequeno);
    color: var(--cor-cinza-escuro);
    margin-bottom: var(--espacamento-pequeno);
  }

  &__nome {
    font-size: var(--fonte-tamanho-medio);
    margin-bottom: var(--espacamento-pequeno);
    font-weight: 500;
  }

  &__preco {
    font-size: var(--fonte-tamanho-medio);
    font-weight: 700;
    margin-top: auto;
    display: flex;
    gap: 0.5rem;
    align-items: center;

    .preco-original {
      text-decoration: line-through;
      color: var(--cor-cinza);
      font-size: 0.9em;
    }

    .preco-promocional {
      color: var(--cor-primaria);
      font-weight: bold;
    }
  }

  &__acoes {
    padding: 0 var(--espacamento-medio) var(--espacamento-medio);
  }

  &__botao {
    width: 100%;
    
    &:disabled {
      background-color: var(--cor-cinza);
      cursor: not-allowed;

      &:hover {
        background-color: var(--cor-cinza);
      }
    }
  }
}
</style>
