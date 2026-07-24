import { useEffect, useState } from 'react'
import './App.css'

import {
  getExpenses,
  addExpense,
  updateExpense,
  deleteExpense,
  deleteAllExpenses,
  seedExpenses
} from './services/expenseService'

function App() {
  const [expenses, setExpenses] = useState([])
  const [title, setTitle] = useState('')
  const [amount, setAmount] = useState('')
  const [date, setDate] = useState('')
  const [editingId, setEditingId] = useState(null)

  useEffect(() => {
    loadExpenses()
  }, [])

  async function loadExpenses() {
    try {
      const data = await getExpenses()
      setExpenses(data)
    } catch (error) {
      console.error(error)
      alert(error.message)
    }
  }

  async function handleAddExpense() {
    if (!title || !amount || !date) {
      alert('Please fill out all fields')
      return
    }

    if (Number(amount) <= 0) {
      alert('Amount must be greater than zero')
      return
    }

    if (title.length > 50) {
      alert('Title must be 50 characters or less')
      return
    }

    const newExpense = {
      title,
      amount: Number(amount),
      date
    }

    try {
      await addExpense(newExpense)
      clearForm()
      await loadExpenses()
    } catch (error) {
      console.error(error)
      alert(error.message)
    }
  }

  async function handleDeleteExpense(id) {
    try {
      await deleteExpense(id)
      await loadExpenses()
    } catch (error) {
      console.error(error)
      alert(error.message)
    }
  }

  async function handleDeleteAllExpenses() {
    const confirmed = window.confirm(
      'Are you sure you want to delete all expenses?'
    )

    if (!confirmed) {
      return
    }

    try {
      await deleteAllExpenses()
      clearForm()
      await loadExpenses()
    } catch (error) {
      console.error(error)
      alert(error.message)
    }
  }

  function handleEditExpense(expense) {
    setEditingId(expense.id)
    setTitle(expense.title)
    setAmount(expense.amount)

    if (expense.date.includes('T')) {
      setDate(expense.date.split('T')[0])
    } else {
      setDate(expense.date)
    }
  }

  async function handleSeedExpenses() {
    try {
      await seedExpenses()
      await loadExpenses()
    } catch (error) {
      console.error(error)
      alert(error.message)
    }
  }

  async function handleSaveExpense() {
    if (!title || !amount || !date) {
      alert('Please fill out all fields')
      return
    }

    if (Number(amount) <= 0) {
      alert('Amount must be greater than zero')
      return
    }

    if (title.length > 50) {
      alert('Title must be 50 characters or less')
      return
    }

    const updatedExpense = {
      id: editingId,
      title,
      amount: Number(amount),
      date
    }

    try {
      await updateExpense(editingId, updatedExpense)
      clearForm()
      await loadExpenses()
    } catch (error) {
      console.error(error)
      alert(error.message)
    }
  }

  function clearForm() {
    setTitle('')
    setAmount('')
    setDate('')
    setEditingId(null)
  }

  const totalAmount = expenses.reduce(
    (sum, expense) => sum + expense.amount,
    0
  )

  return (
    <div>
      <h1>Expense Tracker</h1>

      <a
        href="https://github.com/shorz123/ExpenseTrackerWeb/blob/main/README.md"
        target="_blank"
        rel="noopener noreferrer"
      >
        View GitHub Source Code
      </a>

      <p>
        Technologies: React | ASP.NET Core | Entity Framework Core | PostgreSQL
      </p>

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
          <button onClick={handleAddExpense}>
            Add Expense
          </button>
        ) : (
          <button onClick={handleSaveExpense}>
            Save Expense
          </button>
        )}

        <button onClick={clearForm}>
          Clear
        </button>
      </div>

      <h3>
        Expenses Total: ${totalAmount.toFixed(2)}
      </h3>

      <button onClick={handleDeleteAllExpenses}>
        Delete All Expenses
      </button>

      <button onClick={handleSeedExpenses}>
        Add Demo Data
      </button>

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

              <td>
                ${Number(expense.amount).toFixed(2)}
              </td>

              <td>
                {new Date(expense.date).toLocaleDateString()}
              </td>

              <td>
                <button
                  onClick={() => handleEditExpense(expense)}
                >
                  Edit
                </button>

                <button
                  onClick={() =>
                    handleDeleteExpense(expense.id)
                  }
                >
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