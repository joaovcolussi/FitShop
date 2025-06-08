import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export interface ProdutoCarrinho {
  id: number
  nome: string
  preco: number
  precoPromocional?: number
  promocao?: boolean
  imagem: string
  quantidade: number
  categoria?: string
}

export const useCarrinhoStore = defineStore('carrinho', () => {
  const itens = ref<ProdutoCarrinho[]>([])

  if (typeof window !== 'undefined') {
    const carrinhoSalvo = localStorage.getItem('carrinho')
    if (carrinhoSalvo) {
      itens.value = JSON.parse(carrinhoSalvo)
    }
  }

  const salvarCarrinho = () => {
    localStorage.setItem('carrinho', JSON.stringify(itens.value))
  }

  const quantidadeTotal = computed(() =>
    itens.value.reduce((total, item) => total + item.quantidade, 0)
  )

  const valorTotal = computed(() =>
    itens.value.reduce((total, item) => {
      const preco = item.promocao && item.precoPromocional
        ? item.precoPromocional
        : item.preco
      return total + preco * item.quantidade
    }, 0)
  )

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
    if (item && quantidade >= 1) {
      item.quantidade = quantidade
      salvarCarrinho()
    }
  }

  const aumentarQuantidade = (produtoId: number) => {
    const item = itens.value.find(i => i.id === produtoId)
    if (item) {
      item.quantidade++
      salvarCarrinho()
    }
  }

  const diminuirQuantidade = (produtoId: number) => {
    const item = itens.value.find(i => i.id === produtoId)
    if (item) {
      if (item.quantidade > 1) {
        item.quantidade--
      } else {
        removerItem(produtoId)
      }
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
      const precoUnitario = item.promocao && item.precoPromocional
        ? item.precoPromocional
        : item.preco
      const subtotal = precoUnitario * item.quantidade

      mensagem += `*${item.nome}*\n`
      mensagem += `Quantidade: ${item.quantidade}\n`
      mensagem += `Preço unitário: ${precoUnitario.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}\n`
      mensagem += `Subtotal: ${subtotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}\n\n`
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
    aumentarQuantidade,
    diminuirQuantidade,
    limparCarrinho,
    enviarParaWhatsApp
  }
})
