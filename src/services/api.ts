import axios from 'axios'

const API_BASE_URL = 'http://localhost:5000/api' 

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

export interface Produto {
  id: number
  nome: string
  descricao?: string
  preco: number
  categoria?: string
  imagem?: string
  estoque: number
  novidade: boolean
  promocao: boolean
  precoPromocional?: number
}

export interface Categoria {
  id: number
  nome: string
  slug: string
}

export interface CreateProduto {
  nome: string
  descricao?: string
  preco: number
  categoria?: string
  imagem?: string
  estoque: number
  novidade: boolean
  promocao: boolean
  precoPromocional?: number
}

export interface UpdateProduto {
  nome?: string
  descricao?: string
  preco?: number
  categoria?: string
  imagem?: string
  estoque?: number
  novidade?: boolean
  promocao?: boolean
  precoPromocional?: number
}

export interface CreateCategoria {
  nome: string
  slug: string
}

export interface UpdateCategoria {
  nome?: string
  slug?: string
}

export interface WhatsAppItem {
  id: number
  nome: string
  preco: number
  quantidade: number
  imagem?: string
}

export interface WhatsAppPedido {
  items: WhatsAppItem[]
  total: number
  telefone: string
}

export default {
  // Produtos
  async getProdutos(filtro: { termo?: string, categoria?: string } = {}): Promise<Produto[]> {
    const params = new URLSearchParams()
    if (filtro.termo) params.append('termo', filtro.termo)
    if (filtro.categoria) params.append('categoria', filtro.categoria)
    
    const response = await api.get(`/produtos?${params.toString()}`)
    return response.data
  },

  async getProduto(id: number): Promise<Produto> {
    const response = await api.get(`/produtos/${id}`)
    return response.data
  },

  async getPromocoes(): Promise<Produto[]> {
    const response = await api.get('/produtos/promocoes')
    return response.data
  },

  async getNovidades(): Promise<Produto[]> {
    const response = await api.get('/produtos/novidades')
    return response.data
  },

  async criarProduto(produto: CreateProduto): Promise<Produto> {
    const response = await api.post('/produtos', produto)
    return response.data
  },

  async atualizarProduto(id: number, produto: UpdateProduto): Promise<void> {
    await api.put(`/produtos/${id}`, produto)
  },

  async excluirProduto(id: number): Promise<void> {
    await api.delete(`/produtos/${id}`)
  },

  // Categorias
  async getCategorias(): Promise<Categoria[]> {
    const response = await api.get('/categorias')
    return response.data
  },

  async getCategoria(id: number): Promise<Categoria> {
    const response = await api.get(`/categorias/${id}`)
    return response.data
  },

  async criarCategoria(categoria: CreateCategoria): Promise<Categoria> {
    const response = await api.post('/categorias', categoria)
    return response.data
  },

  async atualizarCategoria(id: number, categoria: UpdateCategoria): Promise<void> {
    await api.put(`/categorias/${id}`, categoria)
  },

  async excluirCategoria(id: number): Promise<void> {
    await api.delete(`/categorias/${id}`)
  },

  // WhatsApp
  async enviarPedidoWhatsApp(items: WhatsAppItem[], total: number, telefone: string): Promise<{ url: string }> {
    const pedido: WhatsAppPedido = {
      items,
      total,
      telefone
    }
    
    const response = await api.post('/whatsapp/enviar-pedido', pedido)
    return response.data
  }
}