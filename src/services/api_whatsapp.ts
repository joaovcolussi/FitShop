// services/api_whatsapp.js (ou o caminho correto para seu arquivo)
import axios from 'axios'

// semente de dados enquanto não temos o backend em C#
const produtos = [
  {
    id: 1,
    nome: 'Whey Protein Gold Standard',
    descricao: 'Whey Protein de alta qualidade com 24g de proteína por dose. Ideal para recuperação muscular e ganho de massa magra.',
    preco: 149.90,
    categoria: 'Suplementos',
    imagem: 'https://images.pexels.com/photos/8844577/pexels-photo-8844577.jpeg?auto=compress&cs=tinysrgb&w=600',
    estoque: 50,
    novidade: true, // Flag para novidade
    promocao: true,  // Flag para promoção
    precoPromocional: 135.00 // Preço com desconto
  },
  {
    id: 2,
    nome: 'Creatina Monohidratada 300g',
    descricao: 'Creatina pura para aumento de força e resistência durante os treinos. 5g por dose diária.',
    preco: 89.90,
    categoria: 'Suplementos',
    imagem: 'https://images.pexels.com/photos/12304205/pexels-photo-12304205.jpeg?auto=compress&cs=tinysrgb&w=600',
    estoque: 30,
    novidade: false,
    promocao: true,
    precoPromocional: 79.90
  },
  {
    id: 3,
    nome: 'Luvas de Treino Premium',
    descricao: 'Luvas de alta qualidade para proteção das mãos durante treinos de musculação. Material respirável.',
    preco: 59.90,
    categoria: 'Acessórios',
    imagem: 'https://images.pexels.com/photos/6456264/pexels-photo-6456264.jpeg?auto=compress&cs=tinysrgb&w=600',
    estoque: 15,
    novidade: true,
    promocao: false
  },
  {
    id: 4,
    nome: 'Camiseta Dry-Fit Performance',
    descricao: 'Camiseta com tecnologia que absorve o suor e mantém o corpo seco durante a atividade física.',
    preco: 79.90,
    categoria: 'Roupas',
    imagem: 'https://images.pexels.com/photos/6311251/pexels-photo-6311251.jpeg?auto=compress&cs=tinysrgb&w=600',
    estoque: 25,
    novidade: true,
    promocao: false
  },
  {
    id: 5,
    nome: 'Barra de Proteína Crunchy',
    descricao: 'Barra de proteína com 20g de proteína e baixo teor de açúcar. Perfeita para lanches entre refeições.',
    preco: 12.90,
    categoria: 'Suplementos',
    imagem: 'https://images.pexels.com/photos/6249089/pexels-photo-6249089.jpeg?auto=compress&cs=tinysrgb&w=600',
    estoque: 100,
    novidade: false,
    promocao: true,
    precoPromocional: 9.90
  },
  {
    id: 6,
    nome: 'Corda de Pular Profissional',
    descricao: 'Corda de pular com cabo de aço revestido e rolamentos para alta velocidade.',
    preco: 45.90,
    categoria: 'Acessórios',
    imagem: 'https://images.pexels.com/photos/4473608/pexels-photo-4473608.jpeg?auto=compress&cs=tinysrgb&w=600',
    estoque: 20,
    novidade: true,
    promocao: false
  },
  {
    id: 7,
    nome: 'Legging Compressão Feminina',
    descricao: 'Legging de compressão para mulheres com tecido respirável e cintura alta.',
    preco: 89.90,
    categoria: 'Roupas',
    imagem: 'https://images.pexels.com/photos/6550839/pexels-photo-6550839.jpeg?auto=compress&cs=tinysrgb&w=600',
    estoque: 30,
    novidade: true,
    promocao: true,
    precoPromocional: 75.00
  },
  {
    id: 8,
    nome: 'BCAA 2:1:1 - 60 cápsulas',
    descricao: 'Suplemento de aminoácidos para preservação muscular e melhor recuperação.',
    preco: 69.90,
    categoria: 'Suplementos',
    imagem: 'https://images.pexels.com/photos/4464819/pexels-photo-4464819.jpeg?auto=compress&cs=tinysrgb&w=600',
    estoque: 40,
    novidade: false,
    promocao: false
  }
]

const categorias = [
  { id: 1, nome: 'Suplementos', slug: 'suplementos' },
  { id: 2, nome: 'Acessórios', slug: 'acessorios' },
  { id: 3, nome: 'Roupas Fitness', slug: 'roupas' },
  { id: 4, nome: 'Equipamentos', slug: 'equipamentos' },
  { id: 5, nome: 'Nutrição', slug: 'nutricao' }
]

// Esta api simulada implementa as operações que seriam feitas por um backend real
export default {
  // Produtos
  getProdutos: async (filtro: { termo?: string, categoria?: string } = {}) => {
    // Simulando um delay de rede
    await new Promise(resolve => setTimeout(resolve, 300))
    
    let resultado = [...produtos]
    
    if (filtro.termo) {
      const termoLowerCase = filtro.termo.toLowerCase()
      resultado = resultado.filter(produto => 
        produto.nome.toLowerCase().includes(termoLowerCase) || 
        produto.descricao.toLowerCase().includes(termoLowerCase)
      )
    }
    
    if (filtro.categoria) {
      resultado = resultado.filter(produto => 
        produto.categoria.toLowerCase() === filtro.categoria?.toLowerCase()
      )
    }
    
    return resultado
  },
  
  getProduto: async (id: number) => {
    // Simulando um delay de rede
    await new Promise(resolve => setTimeout(resolve, 300))
    
    const produto = produtos.find(p => p.id === id)
    
    if (!produto) {
      throw new Error('Produto não encontrado')
    }
    
    return produto
  },

  getPromocoes: async (filtro: { termo?: string, categoria?: string } = {}) => {
    await new Promise(resolve => setTimeout(resolve, 300));
    let resultado = produtos.filter(p => p.promocao);

    if (filtro.termo) {
      const termoLowerCase = filtro.termo.toLowerCase();
      resultado = resultado.filter(produto =>
        produto.nome.toLowerCase().includes(termoLowerCase) ||
        produto.descricao.toLowerCase().includes(termoLowerCase)
      );
    }
    // Poderia adicionar filtro de categoria para promoções também, se necessário
    // if (filtro.categoria) { ... } 
    return resultado;
  },

  getNovidades: async (filtro: { termo?: string, categoria?: string } = {}) => {
    await new Promise(resolve => setTimeout(resolve, 300));
    let resultado = produtos.filter(p => p.novidade);

    if (filtro.termo) {
      const termoLowerCase = filtro.termo.toLowerCase();
      resultado = resultado.filter(produto =>
        produto.nome.toLowerCase().includes(termoLowerCase) ||
        produto.descricao.toLowerCase().includes(termoLowerCase)
      );
    }
    // Poderia adicionar filtro de categoria para novidades também, se necessário
    // if (filtro.categoria) { ... }
    return resultado;
  },
  
  // Admin - Produtos
  criarProduto: async (produto: any) => {
    // Simulando um delay de rede
    await new Promise(resolve => setTimeout(resolve, 500))
    
    const novoProduto = {
      ...produto,
      id: Math.max(...produtos.map(p => p.id)) + 1
    }
    
    produtos.push(novoProduto)
    return novoProduto
  },
  
  atualizarProduto: async (id: number, produto: any) => {
    // Simulando um delay de rede
    await new Promise(resolve => setTimeout(resolve, 500))
    
    const index = produtos.findIndex(p => p.id === id)
    
    if (index === -1) {
      throw new Error('Produto não encontrado')
    }
    
    produtos[index] = { ...produtos[index], ...produto }
    return produtos[index]
  },
  
  excluirProduto: async (id: number) => {
    // Simulando um delay de rede
    await new Promise(resolve => setTimeout(resolve, 500))
    
    const index = produtos.findIndex(p => p.id === id)
    
    if (index === -1) {
      throw new Error('Produto não encontrado')
    }
    
    const produtoRemovido = produtos.splice(index, 1)[0]
    return produtoRemovido
  },
  
  // Categorias
  getCategorias: async () => {
    // Simulando um delay de rede
    await new Promise(resolve => setTimeout(resolve, 300))
    
    return categorias
  },
  
  // Admin - Categorias (código existente omitido para brevidade)
  criarCategoria: async (categoria: any) => { /* ... */ return categoria },
  atualizarCategoria: async (id: number, categoria: any) => { /* ... */ return categoria },
  excluirCategoria: async (id: number) => { /* ... */ return {} },
  
  // Este método simula uma integração com WhatsApp que seria feita pelo backend
  enviarPedidoWhatsApp: async (items: any[], total: number, telefone: string) => {
    // Em um backend real, aqui poderia ser feita uma integração com a API do WhatsApp
    console.log('Enviando pedido via WhatsApp', { items, total, telefone })
    
    // Retorna a URL para redirecionamento (no frontend real, isso seria construído no backend)
    let mensagem = 'Olá! A loja fitshop recebeu seu pedido, gostaria de seguir com ele:\n\n'
    
    items.forEach(item => {
      mensagem += `*${item.nome}*\n`
      mensagem += `Quantidade: ${item.quantidade}\n`
      mensagem += `Preço unitário: ${(item.preco).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}\n`
      mensagem += `Subtotal: ${(item.preco * item.quantidade).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}\n\n`
    })
    
    mensagem += `*Total do pedido: ${total.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}*\n\n`
    
    const numeroFormatado = telefone.replace(/\D/g, '')
    const url = `https://wa.me/${numeroFormatado}?text=${encodeURIComponent(mensagem)}`
    
    return { url }
  }
}