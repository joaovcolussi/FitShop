<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import api_whatsapp from '../services/api'

const route = useRoute()
const produto = ref<any | null>(null)
const erro = ref('')

const carregarProduto = async () => {
  try {
    const id = route.params.id
    if (!id) {
      erro.value = 'ID não informado'
      return
    }

    const produtos = await api_whatsapp.getProdutos()
    produto.value = produtos.find((p: any) => String(p.id) === String(id))

    if (!produto.value) {
      erro.value = 'Produto não encontrado.'
    }
  } catch (err) {
    erro.value = 'Erro ao carregar produto.'
    console.error(err)
  }
}

const formatarMoeda = (valor: number): string => {
  return valor.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  })
}

const precoAtualFormatado = computed(() => {
  if (!produto.value) return ''
  const precoParaExibir =
    produto.value.promocao && typeof produto.value.precoPromocional === 'number'
      ? produto.value.precoPromocional
      : produto.value.preco
  return formatarMoeda(precoParaExibir)
})

onMounted(() => {
  carregarProduto()
})
</script>

<template>
  <div class="pagina-produto container">
    <div v-if="erro">
      <p>{{ erro }}</p>
    </div>

    <div v-else-if="produto">
      <h1 class="titulo">{{ produto.nome }}</h1>

      <div class="produto__info">
        <img :src="produto.imagem" :alt="produto.nome" class="produto__imagem" />
        <div class="produto__detalhes">
          <p class="produto__preco">{{ precoAtualFormatado }}</p>
          <p class="produto__descricao">{{ produto.descricao }}</p>
        </div>
      </div>
    </div>

    <div v-else>
      <p>Carregando produto...</p>
    </div>
  </div>
</template>

<style scoped>
.pagina-produto {
  padding: 2rem;
}

.titulo {
  font-size: 2rem;
  margin-bottom: 1.5rem;
}

.produto__info {
  display: flex;
  gap: 2rem;
}

.produto__imagem {
  width: 300px;
  height: auto;
  object-fit: contain;
}

.produto__detalhes {
  flex-grow: 1;
}

.produto__preco {
  font-size: 1.5rem;
  color: #27ae60;
  margin-bottom: 1rem;
}

.produto__descricao {
  font-size: 1rem;
}
</style>
