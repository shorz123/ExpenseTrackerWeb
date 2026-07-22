const API_URL = 'http://localhost:5113/api/v1/expenses'

export async function getExpenses() {
    const response = await fetch(API_URL)
    return await response.json()
}

export async function addExpense(expense) {
    await fetch(API_URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(expense)
    })
}

export async function updateExpense(id, expense) {
    await fetch(`${API_URL}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(expense)
    })
}

export async function deleteExpense(id) {
    await fetch(`${API_URL}/${id}`, {
        method: 'DELETE'
    })
}

export async function deleteAllExpenses() {
    const response = await fetch(`${API_URL}/all`, {
        method: 'DELETE'
    })

    if (!response.ok) {
        throw new Error('Failed to delete all expenses')
    }
}

