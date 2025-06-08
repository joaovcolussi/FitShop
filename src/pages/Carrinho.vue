<template>
  <div class="carrinho">
    <h1>Meu Carrinho</h1>
    <div class="carrinho__container">
      <section class="carrinho__produtos">
        <header>
          <h2>Produtos</h2>
          <button class="limpar" @click="carrinho.limparCarrinho">Limpar carrinho</button>
        </header>

        <div v-if="carrinho.itens.length === 0">
          <p>Seu carrinho está vazio.</p>
        </div>

        <div v-else v-for="item in carrinho.itens" :key="item.id" class="produto">
          <img :src="item.imagem" class="produto__img" />
          <strong>{{ item.nome }}</strong>

          <div class="produto__qtd">
            <button @click="diminuir(item.id)">-</button>
            <span>{{ item.quantidade }}</span>
            <button @click="aumentar(item.id)">+</button>
          </div>

          <div class="produto__preco">
            <span v-if="item.promocao && item.precoPromocional">
              <s>{{ formatar(item.preco) }}</s>
              <strong>{{ formatar(item.precoPromocional) }}</strong>
            </span>
            <span v-else><strong>{{ formatar(item.preco) }}</strong></span>
          </div>

          <button class="remover" @click="carrinho.removerItem(item.id)">✕</button>
        </div>

        <router-link class="continuar" to="/">Continuar comprando</router-link>
      </section>

      <section class="carrinho__resumo">
        <h2>Resumo do pedido</h2>
        <div class="linha">
          <span>Subtotal</span>
          <span>{{ formatar(subtotal) }}</span>
        </div>
        <div class="linha total">
          <span>Total</span>
          <span>{{ formatar(total) }}</span>
        </div>

        <label class="telefone-label">
          <strong>Telefone com DDD</strong>
          <input v-model="telefone" placeholder="(11) 99999-9999" />
        </label>

        <button class="whatsapp" @click="enviarWhatsApp">Enviar pedido via WhatsApp</button>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useCarrinhoStore } from '../stores/carrinho'

const carrinho = useCarrinhoStore()
const telefone = ref('')

const formatar = (valor: number) => valor.toLocaleString('pt-BR', {
  style: 'currency',
  currency: 'BRL'
})

const subtotal = computed(() => {
  return carrinho.itens.reduce((soma, item) => soma + item.preco * item.quantidade, 0)
})

const total = computed(() => {
  return carrinho.itens.reduce((soma, item) => {
    const preco = item.promocao && item.precoPromocional ? item.precoPromocional : item.preco
    return soma + preco * item.quantidade
  }, 0)
})

const aumentar = (id: number) => {
  carrinho.aumentarQuantidade(id)
}

const diminuir = (id: number) => {
  carrinho.diminuirQuantidade(id)
}

const enviarWhatsApp = () => {
  if (!telefone.value) {
    alert('Digite um telefone válido.')
    return
  }

  const mensagem = carrinho.itens.map(item => {
    const preco = item.promocao && item.precoPromocional ? item.precoPromocional : item.preco
    return `• ${item.nome} - ${item.quantidade} x ${formatar(preco)}`
  }).join('%0A')

  const url = `https://wa.me/55${telefone.value.replace(/\D/g, '')}?text=Pedido:%0A${mensagem}%0A%0ATotal: ${formatar(total.value)}`
  window.open(url, '_blank')
}
</script>

<style scoped>
.carrinho {
  padding: 2rem;
}
.carrinho__container {
  display: flex;
  flex-wrap: wrap;
  gap: 2rem;
}
.carrinho__produtos {
  flex: 1;
  min-width: 300px;
  background: #fff;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 0 10px rgba(0,0,0,0.05);
}
.carrinho__produtos header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 1rem;
}
.produto {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
}
.produto__img {
  width: 60px;
  height: 60px;
  object-fit: cover;
  border-radius: 4px;
}
.produto__qtd button {
  width: 24px;
  height: 24px;
}
.produto__qtd {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}
.produto__preco {
  margin-left: auto;
  text-align: right;
}
.remover {
  background: none;
  border: none;
  color: red;
  font-size: 18px;
  cursor: pointer;
}
.continuar {
  display: inline-block;
  margin-top: 1rem;
  text-decoration: underline;
}
.carrinho__resumo {
  width: 300px;
  background: #fff;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 0 10px rgba(0,0,0,0.05);
}
.linha {
  display: flex;
  justify-content: space-between;
  margin: 0.75rem 0;
}
.total {
  font-weight: bold;
  color: #222;
}
.telefone-label {
  display: flex;
  flex-direction: column;
  margin: 1rem 0;
}
input {
  padding: 0.5rem;
  font-size: 1rem;
  margin-top: 0.25rem;
}
.whatsapp {
  background: #25d366;
  color: white;
  border: none;
  padding: 0.75rem;
  font-size: 1rem;
  width: 100%;
  cursor: pointer;
  border-radius: 5px;
}
.limpar {
  background: none;
  border: none;
  color: #e53935;
  cursor: pointer;
}
</style>