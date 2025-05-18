<script setup lang="ts">
import { defineProps, computed } from 'vue'
import { useRouter } from 'vue-router'

// Definindo as propriedades que o card espera receber
const props = defineProps<{
  produto: {
    id: number
    nome: string
    preco: number
    imagem: string
    categoria?: string 
    descricao?: string
    promocao?: boolean
    precoPromocional?: number 
   
  }
}>()

const router = useRouter()

// Função para formatar valores monetários
const formatarMoeda = (valor: number): string => {
  if (typeof valor !== 'number') return ''
  return valor.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  })
}

// Preço atual a ser exibido (promocional ou normal)
const precoAtualFormatado = computed(() => {
  if (!props.produto) return ''
  const precoParaExibir = (props.produto.promocao && typeof props.produto.precoPromocional === 'number')
    ? props.produto.precoPromocional
    : props.produto.preco
  return formatarMoeda(precoParaExibir)
})


const precoOriginalFormatado = computed(() => {
  if (!props.produto || !props.produto.promocao || typeof props.produto.precoPromocional !== 'number') {
    return '' 
  }
  return formatarMoeda(props.produto.preco)
})

// Navega para a página de detalhes do produto
const irParaDetalhes = () => {
  if (props.produto && props.produto.id) {
    router.push(`/produto/${props.produto.id}`) // Ajuste esta rota se necessário
  }
}
</script>

<template>
  <div class="produto-card" @click="irParaDetalhes" v-if="produto" :aria-label="`Ver detalhes de ${produto.nome}`">
    <div class="produto-card__imagem-container">
      <img :src="produto.imagem" :alt="produto.nome" class="produto-card__imagem" loading="lazy" />
      <span v-if="produto.promocao" class="produto-card__tag-promocao">OFERTA</span>
    </div>
    <div class="produto-card__info">
      <h3 class="produto-card__nome" :title="produto.nome">{{ produto.nome }}</h3>
      <div class="produto-card__precos">
        <span v-if="produto.promocao && precoOriginalFormatado" class="produto-card__preco-original">
          {{ precoOriginalFormatado }}
        </span>
        <span class="produto-card__preco-atual">
          {{ precoAtualFormatado }}
        </span>
      </div>
      </div>
  </div>
</template>

<style lang="scss" scoped>
:root {
  --cor-primaria: #007bff;
  --cor-secundaria: #ff4081;
  --cor-texto-principal: #333;
  --cor-cinza-claro: #f0f0f0;
  --cor-cinza-escuro: #757575;
  --cor-branco: #fff;
  --borda-raio: 8px;
  --borda-raio-pequeno: 4px;
}

.produto-card {
  border: 1px solid var(--cor-cinza-claro, #e0e0e0);
  border-radius: var(--borda-raio, 8px);
  background-color: var(--cor-branco, #fff);
  display: flex;
  flex-direction: column;
  height: 100%; 
  cursor: pointer;
  transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
  overflow: hidden; 

  &:hover, &:focus-within {
    transform: translateY(-5px);
    box-shadow: 0 8px 16px rgba(0, 0, 0, 0.12);
  }

  &__imagem-container {
    width: 100%;
    padding-top: 100%; 
    position: relative;
    background-color: var(--cor-cinza-claro, #f8f8f8);
  }

  &__imagem {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
  
  &__tag-promocao {
    position: absolute;
    top: 10px;
    right: 10px;
    background-color: var(--cor-secundaria, #ff4081);
    color: var(--cor-branco, #fff);
    padding: 5px 10px;
    font-size: 0.78rem;
    font-weight: bold;
    border-radius: var(--borda-raio-pequeno, 4px);
    z-index: 1;
    text-transform: uppercase;
  }

  &__info {
    padding: 12px 16px;
    text-align: left;
    display: flex;
    flex-direction: column;
    flex-grow: 1; 
  }

  &__nome {
    font-size: 1rem; 
    font-weight: 600; 
    color: var(--cor-texto-principal, #333);
    margin-bottom: 8px; 
    display: -webkit-box;
    -webkit-line-clamp: 2; 
    -webkit-box-orient: vertical;
    overflow: hidden;
    text-overflow: ellipsis;
    min-height: 2.4em; 
    line-height: 1.2em;
  }

  &__precos {
    display: flex;
    flex-wrap: wrap; 
    align-items: baseline; 
    gap: 8px; 
    margin-bottom: 10px; 
  }

  &__preco-original {
    font-size: 0.85rem;
    color: var(--cor-cinza-escuro, #757575);
    text-decoration: line-through;
  }

  &__preco-atual {
    font-size: 1.15rem; 
    font-weight: bold;
    color: var(--cor-primaria, #007bff); 
  }

  &__categoria { 
    font-size: 0.8rem;
    color: var(--cor-cinza-escuro, #757575);
    margin-bottom: auto; 
    padding-bottom: 8px; 
  }
  
  &__botao-comprar { 
    margin-top: auto; 
    width: 100%;

  }
}
</style>