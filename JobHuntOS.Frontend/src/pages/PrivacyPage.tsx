import { Link } from 'react-router-dom'

const PrivacyPage = () => (
    <div className="min-h-screen bg-gray-50 py-12 px-4">
        <div className="max-w-2xl mx-auto bg-white border border-gray-200 rounded-lg p-8 space-y-4 text-sm text-gray-700 leading-relaxed">
            <Link to="/login" className="text-blue-600 hover:underline">Back</Link>
            <h1 className="text-2xl font-bold text-gray-900">Privacy Notice</h1>
            <p className="text-gray-500">Last updated: 2 November 2026</p>

            <p>
                Job Hunt OS is a personal portfolio project, not a commercial service. It is a demo,
                so please use sample data and do not enter your real CV or sensitive personal information.
            </p>

            <h2 className="text-lg font-semibold text-gray-900 pt-2">Who is responsible</h2>
            <p>
                The project is run by Ahmed Elhussein. Contact: YOUR_EMAIL_HERE
            </p>

            <h2 className="text-lg font-semibold text-gray-900 pt-2">What is stored</h2>
            <ul className="list-disc pl-6 space-y-1">
                <li>Your email address, and your password stored as a hash (the password itself is not readable)</li>
                <li>The CV text you paste into Settings</li>
                <li>The job applications and notes you add</li>
                <li>The results of analyses you run</li>
            </ul>

            <h2 className="text-lg font-semibold text-gray-900 pt-2">Why it is stored</h2>
            <p>
                Only to provide the features of the app: logging you in, showing your own data back to you,
                and running the fit analysis. Your data is not sold and is not used for advertising.
            </p>

            <h2 className="text-lg font-semibold text-gray-900 pt-2">Who it is shared with</h2>
            <ul className="list-disc pl-6 space-y-1">
                <li>
                    Microsoft Azure hosts the app and its database. The database is in the UK.
                </li>
                <li>
                    Anthropic provides the AI. When you run an analysis, your CV text and the job description
                    you paste are sent to the Anthropic API to produce the result. Anthropic handles that data
                    under its own terms and privacy policy, which you should read before using this feature.
                </li>
            </ul>

            <h2 className="text-lg font-semibold text-gray-900 pt-2">Cookies and tracking</h2>
            <p>
                There are no advertising or analytics cookies. A login token is kept in your browser
                storage so you stay logged in, and it is removed when you log out.
            </p>

            <h2 className="text-lg font-semibold text-gray-900 pt-2">How long it is kept</h2>
            <p>
                Data is kept while your account exists. This is a demo, so accounts and data may be deleted
                at any time, including when the hosting ends.
            </p>

            <h2 className="text-lg font-semibold text-gray-900 pt-2">Your rights</h2>
            <p>
                Under UK data protection law you can ask to see, correct or delete your data. Email the
                address above and it will be dealt with by hand. If you are unhappy with how your data
                is handled, you can complain to the Information Commissioner&apos;s Office (ico.org.uk).
            </p>

            <p className="pt-2">
                <Link to="/terms" className="text-blue-600 hover:underline">Terms of Use</Link>
            </p>
        </div>
    </div>
)

export default PrivacyPage
