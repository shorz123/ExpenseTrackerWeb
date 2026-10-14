const API_URL = 'http://localhost:5113/api/v1/expenses'

async function handleResponse(response, fallbackMessage) {
    if (response.status === 429) {
        throw new Error('Too many requests. Please wait a minute and try again.')
    }

    if (!response.ok) {
        let message = fallbackMessage

        // The API returns validation errors as JSON strings.
        if (response.status === 400) {
            try {
                const error = await response.json()

                if (typeof error === 'string' && error.trim()) {
                    message = error
                }
            } catch {
                // Keep the fallback if the response isn't valid JSON.
            }
        }

        throw new Error(message)
    }

    return response
}

export async function getExpenses() {
    const response = await fetch(API_URL)

    await handleResponse(response, 'Unable to load expenses')

    return await response.json()
}

export async function addExpense(expense) {
    const response = await fetch(API_URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(expense)
    })

    await handleResponse(response, 'Unable to add expense')
}

export async function updateExpense(id, expense) {
    const response = await fetch(`${API_URL}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(expense)
    })

    await handleResponse(response, 'Unable to update expense')
}

export async function deleteExpense(id) {
    const response = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE'
    })

    await handleResponse(response, 'Unable to delete expense')
}

export async function deleteAllExpenses() {
    const response = await fetch(`${API_URL}/all`, {
        method: 'DELETE'
    })

    await handleResponse(response, 'Unable to delete all expenses')
}

export async function seedExpenses() {
    const response = await fetch(`${API_URL}/seed`, {
        method: 'POST'
    })

    await handleResponse(response, 'Unable to add demo expenses')

    return await response.json()
}