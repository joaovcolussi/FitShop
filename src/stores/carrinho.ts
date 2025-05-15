import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export interface ProdutoCarrinho {
  id: number
  nome: string
  preco: number
  imagem: string
  quantidade: number
  categoria?: string
}

export const useCarrinhoStore = defineStore('carrinho', () => {
  const itens = ref<ProdutoCarrinho[]>([])
  
  // Carregar itens do localStorage quando o store é inicializado
  if (typeof window !== 'undefined') {
    const carrinhoSalvo = localStorage.getItem('carrinho')
    if (carrinhoSalvo) {
      itens.value = JSON.parse(carrinhoSalvo)
    }
  }
  
  // Persistir mudanças no localStorage
  const salvarCarrinho = () => {
    localStorage.setItem('carrinho', JSON.stringify(itens.value))
  }
  
  // Getters
  const quantidadeTotal = computed(() => {
    return itens.value.reduce((total, item) => total + item.quantidade, 0)
  })
  
  const valorTotal = computed(() => {
    return itens.value.reduce((total, item) => total + (item.preco * item.quantidade), 0)
  })
  
  // Ações
  const adicionarItem = (produto: ProdutoCarrinho) => {
    const itemExistente = itens.value.find(item => item.id === produto.id)
    
    if (itemExistente) {
      itemExistente.quantidade += produto.quantidade
    } else {
      itens.value.push({ ...produto })
    }
    
    salvarCarrinho()
  }
  
  const removerItem = (produtoId: number) => {
    const index = itens.value.findIndex(item => item.id === produtoId)
    if (index !== -1) {
      itens.value.splice(index, 1)
      salvarCarrinho()
    }
  }
  
  const atualizarQuantidade = (produtoId: number, quantidade: number) => {
    const item = itens.value.find(item => item.id === produtoId)
    if (item) {
      item.quantidade = quantidade
      salvarCarrinho()
    }
  }
  
  const limparCarrinho = () => {
    itens.value = []
    salvarCarrinho()
  }
  
  const enviarParaWhatsApp = (telefone: string) => {
    if (itens.value.length === 0) return ''
    
    const numeroFormatado = telefone.replace(/\D/g, '')
    
    let mensagem = 'Olá! Gostaria de fazer o seguinte pedido:\n\n'
    
    itens.value.forEach(item => {
      mensagem += `*${item.nome}*\n`
      mensagem += `Quantidade: ${item.quantidade}\n`
      mensagem += `Preço unitário: ${(item.preco).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}\n`
      mensagem += `Subtotal: ${(item.preco * item.quantidade).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}\n\n`
    })
    
    mensagem += `*Total do pedido: ${valorTotal.value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}*\n\n`
    
    const url = `https://wa.me/${numeroFormatado}?text=${encodeURIComponent(mensagem)}`
    return url
  }
  
  return {
    itens,
    quantidadeTotal,
    valorTotal,
    adicionarItem,
    removerItem,
    atualizarQuantidade,
    limparCarrinho,
    enviarParaWhatsApp
  }
})