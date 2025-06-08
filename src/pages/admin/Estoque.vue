<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api_whatsapp from '../../services/api'

const produtos = ref<any[]>([])
const carregando = ref(true)
const erro = ref('')
const sucesso = ref(false)
const atualizandoEstoque = ref(false)

const carregarProdutos = async () => {
  carregando.value = true
  try {
    const response = await api_whatsapp.getProdutos()
    produtos.value = response
  } catch (error) {
    console.error('Erro ao carregar produtos', error)
    erro.value = 'Erro ao carregar produtos.'
  } finally {
    carregando.value = false
  }
}

onMounted(() => {
  carregarProdutos()
})

const atualizarEstoque = async (produto: any) => {
  if (atualizandoEstoque.value) return

  atualizandoEstoque.value = true
  erro.value = ''

  try {
    await api_whatsapp.atualizarProduto(produto.id, { estoque: produto.estoque })
    sucesso.value = true
    setTimeout(() => {
      sucesso.value = false
    }, 3000)
  } catch (error) {
    console.error('Erro ao atualizar estoque', error)
    erro.value = 'Erro ao atualizar o estoque.'
    setTimeout(() => {
      erro.value = ''
    }, 3000)
  } finally {
    atualizandoEstoque.value = false
  }
}

const aumentarEstoque = (produto: any) => {
  produto.estoque++
  atualizarEstoque(produto)
}

const diminuirEstoque = (produto: any) => {
  if (produto.estoque > 0) {
    produto.estoque--
    atualizarEstoque(produto)
  }
}
</script>

<template>
  <div class="estoque container">
    <h1>Gerenciar Estoque</h1>

    <div v-if="carregando">Carregando produtos...</div>
    <div v-else>
      <table class="tabela" v-if="produtos.length">
        <thead>
          <tr>
            <th>Produto</th>
            <th>Estoque</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="produto in produtos" :key="produto.id">
            <td>{{ produto.nome }}</td>
            <td>{{ produto.estoque }}</td>
            <td>
              <button @click="diminuirEstoque(produto)">-1</button>
              <button @click="aumentarEstoque(produto)">+1</button>
            </td>
          </tr>
        </tbody>
      </table>
      <p v-else>Nenhum produto encontrado.</p>

      <div v-if="sucesso" class="mensagem sucesso">Estoque atualizado com sucesso!</div>
      <div v-if="erro" class="mensagem erro">{{ erro }}</div>
    </div>
  </div>
</template>

<style scoped>
.container {
  max-width: 800px;
  margin: auto;
  padding: 2rem;
}
.tabela {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1rem;
}
.tabela th, .tabela td {
  border: 1px solid #ccc;
  padding: 0.5rem;
  text-align: left;
}
button {
  margin: 0 0.3rem;
}
.mensagem.sucesso {
  color: green;
  margin-top: 1rem;
}
.mensagem.erro {
  color: red;
  margin-top: 1rem;
}
</style>