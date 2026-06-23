import { useEffect, useState } from 'react'
import './App.css'

const API_URL = 'http://localhost:5113/expenses'

function App() {
  const [expenses, setExpenses] = useState([])
  const [title, setTitle] = useState('')
  const [amount, setAmount] = useState('')
  const [date, setDate] = useState('')
  const [editingId, setEditingId] = useState(null)

  useEffect(() => {
    getExpenses()
  }, [])

  async function getExpenses() {
    const response = await fetch(API_URL)
    const data = await response.json()
    setExpenses(data)
  }

  async function addExpense() {
    if (!title || !amount || !date) {
      alert('Please fill out all fields')
      return
    }

    const newExpense = {
      title,
      amount: Number(amount),
      date
    }

    await fetch(API_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(newExpense)
    })

    clearForm()
    getExpenses()
  }

  async function deleteExpense(id) {
    await fetch(`${API_URL}/${id}`, {
      method: 'DELETE'
    })

    getExpenses()
  }

  function editExpense(expense) {
    setEditingId(expense.id)
    setTitle(expense.title)
    setAmount(expense.amount)

    if (expense.date.includes('T')) {
      setDate(expense.date.split('T')[0])
    } else {
      setDate(expense.date)
    }
  }

  async function saveExpense() {
    if (!title || !amount || !date) {
      alert('Please fill out all fields')
      return
    }

    const updatedExpense = {
      id: editingId,
      title,
      amount: Number(amount),
      date
    }

    await fetch(`${API_URL}/${editingId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(updatedExpense)
    })

    clearForm()
    getExpenses()
  }

  function clearForm() {
    setTitle('')
    setAmount('')
    setDate('')
    setEditingId(null)
  }

  return (
    <div>
      <h1>Expense Tracker</h1>

      <div>
        <input
          type="text"
          placeholder="Expense Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />

        <input
          type="number"
          placeholder="Amount"
          value={amount}
          onChange={(e) => setAmount(e.target.value)}
        />

        <input
          type="date"
          value={date}
          onChange={(e) => setDate(e.target.value)}
        />

        {editingId === null ? (
          <button onClick={addExpense}>Add Expense</button>
        ) : (
          <button onClick={saveExpense}>Save Expense</button>
        )}

        <button onClick={clearForm}>Clear</button>
      </div>

      <table>
        <thead>
          <tr>
            <th>Expense Title</th>
            <th>Amount</th>
            <th>Date</th>
            <th>Action</th>
          </tr>
        </thead>

        <tbody>
          {expenses.map((expense) => (
            <tr key={expense.id}>
              <td>{expense.title}</td>
              <td>${Number(expense.amount).toFixed(2)}</td>
              <td> {new Date(expense.date).toLocaleDateString()} </td>
              <td>
                <button onClick={() => editExpense(expense)}>
                  Edit
                </button>

                <button onClick={() => deleteExpense(expense.id)}>
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default App