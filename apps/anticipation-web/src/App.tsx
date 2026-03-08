import { useEffect, useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import './App.css'

type Anticipation = {
  id: string
  creator_id: string
  valor_solicitado: number
  valor_liquido: number
  data_solicitacao: string
  status: 'pendentef' | 'aprovada' | 'recusada' | string
}

type PagedResult = {
  page: number
  pageSize: number
  totalItems: number
  totalPages: number
  items: Anticipation[]
}

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? ''
const apiDisplay = apiBaseUrl === '' ? 'same-origin (/api)' : apiBaseUrl

function formatCurrency(value: number): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(value)
}

function formatDate(value: string): string {
  return new Date(value).toLocaleString('pt-BR')
}

async function parseProblem(response: Response): Promise<string> {
  try {
    const payload = await response.json()
    return payload.detail ?? payload.title ?? 'Erro inesperado'
  } catch {
    return 'Erro inesperado'
  }
}

function App() {
  const [items, setItems] = useState<Anticipation[]>([])
  const [page, setPage] = useState(1)
  const [pageSize] = useState(10)
  const [totalPages, setTotalPages] = useState(0)
  const [loading, setLoading] = useState(false)
  const [submitting, setSubmitting] = useState(false)
  const [actionId, setActionId] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)

  const [creatorId, setCreatorId] = useState('')
  const [requestedAmount, setRequestedAmount] = useState('')
  const [requestDate, setRequestDate] = useState(() => new Date().toISOString().slice(0, 16))

  const hasPrevious = useMemo(() => page > 1, [page])
  const hasNext = useMemo(() => totalPages > 0 && page < totalPages, [page, totalPages])

  async function loadAnticipations(targetPage = page): Promise<void> {
    setLoading(true)
    setError(null)

    try {
      const response = await fetch(`${apiBaseUrl}/api/v1/anticipations?page=${targetPage}&pageSize=${pageSize}`)
      if (!response.ok) {
        throw new Error(await parseProblem(response))
      }

      const payload = (await response.json()) as PagedResult
      setItems(payload.items)
      setTotalPages(payload.totalPages)
      setPage(payload.page)
    } catch (loadError) {
      const message = loadError instanceof Error ? loadError.message : 'Falha ao carregar solicitacoes'
      setError(message)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    void loadAnticipations(1)
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  async function handleCreate(event: FormEvent<HTMLFormElement>): Promise<void> {
    event.preventDefault()
    setSubmitting(true)
    setError(null)
    setSuccess(null)

    try {
      const parsedAmount = Number(requestedAmount.replace(',', '.'))
      const isoDate = new Date(requestDate).toISOString()

      const response = await fetch(`${apiBaseUrl}/api/v1/anticipations`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          creator_id: creatorId.trim(),
          valor_solicitado: parsedAmount,
          data_solicitacao: isoDate,
        }),
      })

      if (!response.ok) {
        throw new Error(await parseProblem(response))
      }

      setCreatorId('')
      setRequestedAmount('')
      setRequestDate(new Date().toISOString().slice(0, 16))
      setSuccess('Solicitacao criada com sucesso.')
      await loadAnticipations(1)
    } catch (createError) {
      const message = createError instanceof Error ? createError.message : 'Falha ao criar solicitacao'
      setError(message)
    } finally {
      setSubmitting(false)
    }
  }

  async function updateStatus(id: string, action: 'approve' | 'reject'): Promise<void> {
    setActionId(id)
    setError(null)
    setSuccess(null)

    try {
      const response = await fetch(`${apiBaseUrl}/api/v1/anticipations/${id}/${action}`, {
        method: 'PUT',
      })

      if (!response.ok) {
        throw new Error(await parseProblem(response))
      }

      setSuccess(action === 'approve' ? 'Solicitacao aprovada.' : 'Solicitacao recusada.')
      await loadAnticipations(page)
    } catch (actionError) {
      const message = actionError instanceof Error ? actionError.message : 'Falha ao atualizar solicitacao'
      setError(message)
    } finally {
      setActionId(null)
    }
  }

  return (
    <main className="layout">
      <section className="panel intro">
        <p className="eyebrow">Anticipation Desk</p>
        <h1>Gestao de Solicitacoes de Antecipacao</h1>
        <p>
          Painel unico para criar, listar e decidir solicitacoes de antecipacao.
          Integrado com a API em <code>{apiDisplay}</code>.
        </p>
      </section>

      <section className="panel form-panel">
        <h2>Nova Solicitacao</h2>
        <form onSubmit={handleCreate} className="form-grid">
          <label>
            <span>Creator ID</span>
            <input
              value={creatorId}
              onChange={(event) => setCreatorId(event.target.value)}
              required
              placeholder="creator-001"
            />
          </label>

          <label>
            <span>Valor Solicitado (BRL)</span>
            <input
              value={requestedAmount}
              onChange={(event) => setRequestedAmount(event.target.value)}
              required
              type="number"
              min="100.01"
              step="0.01"
              placeholder="1200.50"
            />
          </label>

          <label>
            <span>Data da Solicitacao</span>
            <input
              value={requestDate}
              onChange={(event) => setRequestDate(event.target.value)}
              required
              type="datetime-local"
            />
          </label>

          <button type="submit" disabled={submitting}>
            {submitting ? 'Criando...' : 'Criar Solicitacao'}
          </button>
        </form>
      </section>

      <section className="panel list-panel">
        <div className="list-header">
          <h2>Solicitacoes Existentes</h2>
          <button onClick={() => void loadAnticipations(page)} disabled={loading}>
            {loading ? 'Atualizando...' : 'Atualizar'}
          </button>
        </div>

        {error && <p className="feedback error">{error}</p>}
        {success && <p className="feedback success">{success}</p>}

        <div className="cards">
          {items.map((item) => (
            <article key={item.id} className="card">
              <header>
                <strong>{item.creator_id}</strong>
                <span className={`status status-${item.status}`}>{item.status}</span>
              </header>
              <p><b>Bruto:</b> {formatCurrency(item.valor_solicitado)}</p>
              <p><b>Liquido:</b> {formatCurrency(item.valor_liquido)}</p>
              <p><b>Data:</b> {formatDate(item.data_solicitacao)}</p>

              {item.status === 'pendentef' && (
                <div className="actions">
                  <button
                    className="approve"
                    disabled={actionId === item.id}
                    onClick={() => void updateStatus(item.id, 'approve')}
                  >
                    Aprovar
                  </button>
                  <button
                    className="reject"
                    disabled={actionId === item.id}
                    onClick={() => void updateStatus(item.id, 'reject')}
                  >
                    Reprovar
                  </button>
                </div>
              )}
            </article>
          ))}
          {!loading && items.length === 0 && <p className="empty">Nenhuma solicitacao encontrada.</p>}
        </div>

        <footer className="pager">
          <button disabled={!hasPrevious || loading} onClick={() => void loadAnticipations(page - 1)}>
            Pagina Anterior
          </button>
          <span>Pagina {page} de {totalPages || 1}</span>
          <button disabled={!hasNext || loading} onClick={() => void loadAnticipations(page + 1)}>
            Proxima Pagina
          </button>
        </footer>
      </section>
    </main>
  )
}

export default App
